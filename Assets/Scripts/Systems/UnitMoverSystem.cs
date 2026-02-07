using DotsRts.MonoBehaviours;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace DotsRts.Systems
{
    public partial struct UnitMoverSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GridSystem.GridSystemData>();
        }

        public const float REACHED_TARGET_POSITION_DISTANCE_SQ = 2f;

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var gridSystemData = SystemAPI.GetSingleton<GridSystem.GridSystemData>();

            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var collisionWorld = physicsWorldSingleton.CollisionWorld;

            foreach (var (localTransform,
                         targetPositionPathQueued,
                         targetPositionPathQueuedEnabled,
                         flowFieldPathRequest,
                         flowFieldPathRequestEnabled,
                         unitMover)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<TargetPositionPathQueued>,
                         EnabledRefRW<TargetPositionPathQueued>,
                         RefRW<FlowFieldPathRequest>,
                         EnabledRefRW<FlowFieldPathRequest>,
                         RefRW<UnitMover>>().WithPresent<FlowFieldPathRequest>())
            {
                var raycastInput = new RaycastInput
                {
                    Start = localTransform.ValueRO.Position,
                    End = targetPositionPathQueued.ValueRO.TargetPosition,
                    Filter = new CollisionFilter
                    {
                        BelongsTo = ~0u,
                        CollidesWith = 1u << GameAssets.PATHFINDING_WALLS,
                        GroupIndex = 0
                    }
                };

                if (!collisionWorld.CastRay(raycastInput))
                {
                    unitMover.ValueRW.TargetPosition = targetPositionPathQueued.ValueRO.TargetPosition;
                }
                else
                {
                    flowFieldPathRequest.ValueRW.TargetPosition = targetPositionPathQueued.ValueRO.TargetPosition;
                    flowFieldPathRequestEnabled.ValueRW = true;
                }

                targetPositionPathQueuedEnabled.ValueRW = false;
            }

            foreach (var (localTransform,
                         flowFieldFollower,
                         flowFieldFollowerEnabled,
                         unitMover)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<FlowFieldFollower>,
                         EnabledRefRW<FlowFieldFollower>,
                         RefRW<UnitMover>>())
            {
                var gridPosition = GridSystem.GetGridPosition(localTransform.ValueRO.Position,
                    gridSystemData.GridNodeSize);
                var index = GridSystem.CalculateIndex(gridPosition, gridSystemData.Width);
                var gridNodeEntity = gridSystemData.GridMapArray[flowFieldFollower.ValueRO.GridIndex]
                    .GridEntityArray[index];
                var gridNode = SystemAPI.GetComponent<GridSystem.GridNode>(gridNodeEntity);
                var gridNodeMoveVector = GridSystem.GetWorldMovementVector(gridNode.Vector);

                if (GridSystem.IsWall(gridNode))
                {
                    gridNodeMoveVector = flowFieldFollower.ValueRO.LastMoveVector;
                }
                else
                {
                    flowFieldFollower.ValueRW.LastMoveVector = gridNodeMoveVector;
                }

                unitMover.ValueRW.TargetPosition =
                    GridSystem.GetWorldCenterPosition(gridPosition.x, gridPosition.y, gridSystemData.GridNodeSize) +
                    gridNodeMoveVector * (gridSystemData.GridNodeSize * 2f);

                if (math.distance(localTransform.ValueRO.Position, flowFieldFollower.ValueRO.TargetPosition) <
                    gridSystemData.GridNodeSize)
                {
                    // Target destination
                    unitMover.ValueRW.TargetPosition = localTransform.ValueRO.Position;
                    flowFieldFollowerEnabled.ValueRW = false;
                }

                var raycastInput = new RaycastInput
                {
                    Start = localTransform.ValueRO.Position,
                    End = flowFieldFollower.ValueRO.TargetPosition,
                    Filter = new CollisionFilter
                    {
                        BelongsTo = ~0u,
                        CollidesWith = 1u << GameAssets.PATHFINDING_WALLS,
                        GroupIndex = 0
                    }
                };

                if (!collisionWorld.CastRay(raycastInput))
                {
                    unitMover.ValueRW.TargetPosition = flowFieldFollower.ValueRO.TargetPosition;
                    flowFieldFollowerEnabled.ValueRW = false;
                }
            }

            var unitMoverJob = new UnitMoverJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
            };
            unitMoverJob.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct UnitMoverJob : IJobEntity
    {
        public float DeltaTime;

        public void Execute(
            ref LocalTransform localTransform,
            ref UnitMover unitMover,
            ref PhysicsVelocity physicsVelocity)
        {
            var moveDirection = unitMover.TargetPosition - localTransform.Position;

            var reachedTargetDistanceSq = UnitMoverSystem.REACHED_TARGET_POSITION_DISTANCE_SQ;
            if (math.lengthsq(moveDirection) <= reachedTargetDistanceSq)
            {
                physicsVelocity.Linear = float3.zero;
                physicsVelocity.Angular = float3.zero;
                unitMover.IsMoving = false;
                return;
            }

            unitMover.IsMoving = true;
            moveDirection = math.normalizesafe(moveDirection);
            localTransform.Rotation = math.slerp(localTransform.Rotation,
                quaternion.LookRotation(moveDirection, math.up()),
                DeltaTime * unitMover.RotationSpeed);
            physicsVelocity.Linear = moveDirection * unitMover.MoveSpeed;
            physicsVelocity.Angular = float3.zero;
        }
    }
}