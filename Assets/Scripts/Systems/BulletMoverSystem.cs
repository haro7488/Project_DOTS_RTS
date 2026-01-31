using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Debug = UnityEngine.Debug;

namespace DotsRts.Systems
{
    public partial struct BulletMoverSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (localTransform,
                         bullet,
                         target,
                         entity)
                     in SystemAPI.Query<
                         RefRW<LocalTransform>,
                         RefRO<Bullet>,
                         RefRO<Target>>().WithEntityAccess())
            {
                if(target.ValueRO.TargetEntity == Entity.Null)
                {
                    entityCommandBuffer.DestroyEntity(entity);
                    continue;
                }
                
                var targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.TargetEntity);
                var targetShootVictim = SystemAPI.GetComponent<ShootVictim>(target.ValueRO.TargetEntity);
                var targetPosition = targetLocalTransform.TransformPoint(targetShootVictim.HitLocalPosition);

                var distanceBeforeSq = math.distancesq(localTransform.ValueRO.Position, targetPosition);

                var moveDirection = targetPosition - localTransform.ValueRO.Position;
                moveDirection = math.normalize(moveDirection);

                localTransform.ValueRW.Position += moveDirection * bullet.ValueRO.Speed * SystemAPI.Time.DeltaTime;

                var distanceAfterSq = math.distancesq(localTransform.ValueRO.Position, targetPosition);

                if (distanceBeforeSq < distanceAfterSq)
                {
                    localTransform.ValueRW.Position = targetPosition;
                }
                
                var destroyAfterSq = .2f;
                if (math.distancesq(localTransform.ValueRO.Position, targetPosition) < destroyAfterSq)
                {
                    var targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.TargetEntity);
                    targetHealth.ValueRW.HealthAmount -= bullet.ValueRO.DamageAmount;
                    entityCommandBuffer.DestroyEntity(entity);
                }
            }
        }
    }
}