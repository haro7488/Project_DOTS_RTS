#define GRID_DEBUG
using DotsRts.MonoBehaviours;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace DotsRts.Systems
{
    public partial struct GridSystem : ISystem
    {
        public const int WALL_COST = byte.MaxValue;

        public struct GridSystemData : IComponentData
        {
            public int Width;
            public int Height;
            public float GridNodeSize;
            public GridMap GridMap;
        }

        public struct GridMap
        {
            public NativeArray<Entity> GridEntityArray;
        }

        public struct GridNode : IComponentData
        {
            public int Index;
            public int X;
            public int Y;
            public byte Cost;
            public byte BestCost;
            public float2 Vector;
        }

        private int2 _targetGridPosition;

#if !GRID_DEBUG
        [BurstCompile]
#endif
        public void OnCreate(ref SystemState state)
        {
            var width = 20;
            var height = 10;
            var gridNodeSize = 5f;
            var totalCount = width * height;

            var gridNodeEntityPrefab = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponent<GridNode>(gridNodeEntityPrefab);

            var gridMap = new GridMap();
            gridMap.GridEntityArray = new NativeArray<Entity>(totalCount, Allocator.Persistent);

            state.EntityManager.Instantiate(gridNodeEntityPrefab, gridMap.GridEntityArray);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var index = CalculateIndex(x, y, width);
                    var gridNode = new GridNode
                    {
                        Index = index,
                        X = x,
                        Y = y,
                    };
#if GRID_DEBUG
                    state.EntityManager.SetName(gridMap.GridEntityArray[index], $"GridNode_{x}_{y}");
#endif
                    SystemAPI.SetComponent(gridMap.GridEntityArray[index], gridNode);
                }
            }

            state.EntityManager.AddComponent<GridSystemData>(state.SystemHandle);
            state.EntityManager.SetComponentData(state.SystemHandle,
                new GridSystemData
                {
                    Width = width,
                    Height = height,
                    GridNodeSize = gridNodeSize,
                    GridMap = gridMap,
                }
            );
        }

#if !GRID_DEBUG
        [BurstCompile]
#endif
        public void OnUpdate(ref SystemState state)
        {
            var gridSystemData = SystemAPI.GetComponent<GridSystemData>(state.SystemHandle);

            var gridNodeNativeArray = new NativeArray<RefRW<GridNode>>(
                gridSystemData.Width * gridSystemData.Height, Allocator.Temp);

            for (int x = 0; x < gridSystemData.Width; x++)
            {
                for (int y = 0; y < gridSystemData.Height; y++)
                {
                    var index = CalculateIndex(x, y, gridSystemData.Width);
                    var gridNodeEntity = gridSystemData.GridMap.GridEntityArray[index];
                    var gridNode = SystemAPI.GetComponentRW<GridNode>(gridNodeEntity);

                    gridNodeNativeArray[index] = gridNode;
                    gridNode.ValueRW.Vector = new float2(0, 1);
                    if (x == _targetGridPosition.x && y == _targetGridPosition.y)
                    {
                        // This is the target destination
                        gridNode.ValueRW.Cost = 0;
                        gridNode.ValueRW.BestCost = 0;
                    }
                    else
                    {
                        gridNode.ValueRW.Cost = 1;
                        gridNode.ValueRW.BestCost = byte.MaxValue;
                    }
                }
            }

            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var collisionWorld = physicsWorldSingleton.CollisionWorld;

            var distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);
            for (int x = 0; x < gridSystemData.Width; x++)
            {
                for (int y = 0; y < gridSystemData.Height; y++)
                {
                    if (collisionWorld.OverlapSphere(
                            GetWorldCenterPosition(x, y, gridSystemData.GridNodeSize),
                            gridSystemData.GridNodeSize * 0.5f,
                            ref distanceHitList,
                            new CollisionFilter
                            {
                                BelongsTo = ~0u,
                                CollidesWith = 1u << GameAssets.PATHFINDING_WALLS,
                                GroupIndex = 0,
                            }
                        ))
                    {
                        // There is a wall in this grid position
                        var index = CalculateIndex(x, y, gridSystemData.Width);
                        gridNodeNativeArray[index].ValueRW.Cost = WALL_COST;
                    }
                }
            }

            var gridNodeOpenQueue = new NativeQueue<RefRW<GridNode>>(Allocator.Temp);
            var targetGridNode = gridNodeNativeArray[CalculateIndex(_targetGridPosition, gridSystemData.Width)];
            gridNodeOpenQueue.Enqueue(targetGridNode);

            var safety = 1000;
            while (gridNodeOpenQueue.Count > 0)
            {
                safety--;
                if (safety < 0)
                {
                    Debug.LogError("Safety break");
                    break;
                }

                var currentGridNode = gridNodeOpenQueue.Dequeue();
                var neighborGridNodeList = GetNeighborGridNodeList(currentGridNode, gridNodeNativeArray,
                    gridSystemData.Width, gridSystemData.Height);

                foreach (var neighborGridNode in neighborGridNodeList)
                {
                    if (neighborGridNode.ValueRO.Cost == WALL_COST)
                    {
                        // This is a wall
                        continue;
                    }

                    var newBestCost = (byte)(currentGridNode.ValueRO.BestCost + neighborGridNode.ValueRO.Cost);
                    if (newBestCost < neighborGridNode.ValueRO.BestCost)
                    {
                        neighborGridNode.ValueRW.BestCost = newBestCost;
                        neighborGridNode.ValueRW.Vector = CalculateVector(
                            neighborGridNode.ValueRO.X, neighborGridNode.ValueRO.Y,
                            currentGridNode.ValueRO.X, currentGridNode.ValueRO.Y);

                        gridNodeOpenQueue.Enqueue(neighborGridNode);
                    }
                }

                neighborGridNodeList.Dispose();
            }

            gridNodeOpenQueue.Dispose();
            gridNodeNativeArray.Dispose();

            if (Input.GetMouseButtonDown(0))
            {
                float3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
                var mouseGridPosition = GetGridPosition(mouseWorldPosition, gridSystemData.GridNodeSize);
                if (IsValidGridPosition(mouseGridPosition, gridSystemData.Width, gridSystemData.Height))
                {
                    var index = CalculateIndex(mouseGridPosition.x, mouseGridPosition.y, gridSystemData.Width);
                    var gridNodeEntity = gridSystemData.GridMap.GridEntityArray[index];
                    var gridNode = SystemAPI.GetComponentRW<GridNode>(gridNodeEntity);

                    _targetGridPosition = mouseGridPosition;
                }
            }

#if GRID_DEBUG
            GridSystemDebug.Instance?.InitializeGrid(gridSystemData);
            GridSystemDebug.Instance?.UpdateGrid(gridSystemData);
#endif
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            var gridSystemData = SystemAPI.GetComponentRW<GridSystemData>(state.SystemHandle);
            gridSystemData.ValueRW.GridMap.GridEntityArray.Dispose();
        }

        public static NativeList<RefRW<GridNode>> GetNeighborGridNodeList(
            RefRW<GridNode> currentGridNode,
            NativeArray<RefRW<GridNode>> gridNodeNativeArray,
            int width, int height)
        {
            var neighborGridNodeList = new NativeList<RefRW<GridNode>>(Allocator.Temp);
            var gridNodeX = currentGridNode.ValueRO.X;
            var gridNodeY = currentGridNode.ValueRO.Y;

            var positionLeft = new int2(gridNodeX - 1, gridNodeY + 0);
            var positionRight = new int2(gridNodeX + 1, gridNodeY + 0);
            var positionUp = new int2(gridNodeX + 0, gridNodeY + 1);
            var positionDown = new int2(gridNodeX + 0, gridNodeY - 1);

            var positionLowerLeft = new int2(gridNodeX - 1, gridNodeY - 1);
            var positionLowerRight = new int2(gridNodeX + 1, gridNodeY - 1);
            var positionUpperLeft = new int2(gridNodeX - 1, gridNodeY + 1);
            var positionUpperRight = new int2(gridNodeX + 1, gridNodeY + 1);

            if (IsValidGridPosition(positionLeft, width, height))
            {
                neighborGridNodeList.Add(gridNodeNativeArray[CalculateIndex(positionLeft, width)]);
            }

            if (IsValidGridPosition(positionRight, width, height))
            {
                neighborGridNodeList.Add(gridNodeNativeArray[CalculateIndex(positionRight, width)]);
            }

            if (IsValidGridPosition(positionUp, width, height))
            {
                neighborGridNodeList.Add(gridNodeNativeArray[CalculateIndex(positionUp, width)]);
            }

            if (IsValidGridPosition(positionDown, width, height))
            {
                neighborGridNodeList.Add(gridNodeNativeArray[CalculateIndex(positionDown, width)]);
            }

            if (IsValidGridPosition(positionLowerLeft, width, height))
            {
                neighborGridNodeList.Add(gridNodeNativeArray[CalculateIndex(positionLowerLeft, width)]);
            }

            if (IsValidGridPosition(positionLowerRight, width, height))
            {
                neighborGridNodeList.Add(gridNodeNativeArray[CalculateIndex(positionLowerRight, width)]);
            }

            if (IsValidGridPosition(positionUpperLeft, width, height))
            {
                neighborGridNodeList.Add(gridNodeNativeArray[CalculateIndex(positionUpperLeft, width)]);
            }

            if (IsValidGridPosition(positionUpperRight, width, height))
            {
                neighborGridNodeList.Add(gridNodeNativeArray[CalculateIndex(positionUpperRight, width)]);
            }

            return neighborGridNodeList;
        }

        public static float2 CalculateVector(int fromX, int fromY, int toX, int toY)
        {
            return new float2(toX - fromX, toY - fromY);
        }

        public static int CalculateIndex(int x, int y, int width)
        {
            return x + y * width;
        }

        public static int CalculateIndex(int2 gridPosition, int width)
        {
            return CalculateIndex(gridPosition.x, gridPosition.y, width);
        }

        public static float3 GetWorldPosition(int x, int y, float gridNodeSize)
        {
            return new float3(
                x * gridNodeSize,
                0f,
                y * gridNodeSize);
        }

        public static float3 GetWorldCenterPosition(int x, int y, float gridNodeSize)
        {
            return new float3(
                x * gridNodeSize + gridNodeSize * 0.5f,
                0f,
                y * gridNodeSize + gridNodeSize * 0.5f);
        }

        public static int2 GetGridPosition(float3 worldPosition, float gridNodeSize)
        {
            return new int2(
                (int)(math.floor(worldPosition.x / gridNodeSize)),
                (int)(math.floor(worldPosition.z / gridNodeSize))
            );
        }

        public static bool IsValidGridPosition(int2 gridPosition, int width, int height)
        {
            return gridPosition.x >= 0 && gridPosition.y >= 0 &&
                   gridPosition.x < width && gridPosition.y < height;
        }
    }
}