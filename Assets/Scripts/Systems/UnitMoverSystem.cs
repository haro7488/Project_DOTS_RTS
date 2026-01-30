using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace DotsRts.Systems
{
    public partial struct UnitMoverSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
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
            in UnitMover unitMover,
            ref PhysicsVelocity physicsVelocity)
        {
            var moveDirection = unitMover.TargetPosition - localTransform.Position;

            var reachedTargetDistanceSq = 2f;
            if (math.lengthsq(moveDirection) < reachedTargetDistanceSq)
            {
                physicsVelocity.Linear = float3.zero;
                physicsVelocity.Angular = float3.zero;
                return;
            }

            moveDirection = math.normalizesafe(moveDirection);
            localTransform.Rotation = math.slerp(localTransform.Rotation,
                quaternion.LookRotation(moveDirection, math.up()),
                DeltaTime * unitMover.RotationSpeed);
            physicsVelocity.Linear = moveDirection * unitMover.MoveSpeed;
            physicsVelocity.Angular = float3.zero;
        }
    }
}