#define GRID_DEBUG
using DotsRts.MonoBehaviours;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DotsRts.Systems
{
    public partial struct GridSystem : ISystem
    {
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
            public int X;
            public int Y;
            public byte Data;
        }

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

            if (Input.GetMouseButtonDown(0))
            {
                float3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
                var mouseGridPosition = GetGridPosition(mouseWorldPosition, gridSystemData.GridNodeSize);
                if (IsValidGridPosition(mouseGridPosition, gridSystemData.Width, gridSystemData.Height))
                {
                    var index = CalculateIndex(mouseGridPosition.x, mouseGridPosition.y, gridSystemData.Width);
                    var gridNodeEntity = gridSystemData.GridMap.GridEntityArray[index];
                    var gridNode = SystemAPI.GetComponentRW<GridNode>(gridNodeEntity);
                    gridNode.ValueRW.Data = 1;
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

        public static int CalculateIndex(int x, int y, int width)
        {
            return x + y * width;
        }

        public static float3 GetWorldPosition(int x, int y, float gridNodeSize)
        {
            return new float3(
                x * gridNodeSize,
                0f,
                y * gridNodeSize);
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