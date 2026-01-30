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
            foreach (var (localTransform,
                         unitMover,
                         physicsVelocity) in
                     SystemAPI.Query<
                         RefRW<LocalTransform>,
                         RefRO<UnitMover>,
                         RefRW<PhysicsVelocity>
                     >())
            {
                var moveDirection = unitMover.ValueRO.TargetPosition - localTransform.ValueRO.Position;
                if (math.lengthsq(moveDirection) < 0.01f)
                {
                    physicsVelocity.ValueRW.Linear = float3.zero;
                    continue;
                }

                moveDirection = math.normalizesafe(moveDirection);

                localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation,
                    quaternion.LookRotation(moveDirection, math.up()),
                    SystemAPI.Time.DeltaTime * unitMover.ValueRO.RotationSpeed);
                // localTransform.ValueRW.Position += moveDirection * moveSpeed.ValueRO.Value * SystemAPI.Time.DeltaTime;
                physicsVelocity.ValueRW.Linear = moveDirection * unitMover.ValueRO.MoveSpeed;
                physicsVelocity.ValueRW.Angular = float3.zero;
            }
        }
    }
}