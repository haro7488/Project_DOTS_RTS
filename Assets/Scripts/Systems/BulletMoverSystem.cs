using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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

                var distanceBeforeSq = math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position);

                var moveDirection = targetLocalTransform.Position - localTransform.ValueRO.Position;
                moveDirection = math.normalize(moveDirection);

                localTransform.ValueRW.Position += moveDirection * bullet.ValueRO.Speed * SystemAPI.Time.DeltaTime;

                var distanceAfterSq = math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position);

                if (distanceBeforeSq < distanceAfterSq)
                {
                    localTransform.ValueRW.Position = targetLocalTransform.Position;
                }


                var destroyAfterSq = .2f;
                if (math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position) < destroyAfterSq)
                {
                    var targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.TargetEntity);
                    targetHealth.ValueRW.HealthAmount -= bullet.ValueRO.DamageAmount;

                    entityCommandBuffer.DestroyEntity(entity);
                }
            }
        }
    }
}