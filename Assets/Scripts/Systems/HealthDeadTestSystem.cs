using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace DotsRts.Systems
{
    public partial struct HealthDeadTestSystem : ISystem
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

            foreach (var (health,
                         entity)
                     in SystemAPI.Query<
                         RefRO<Health>>().WithEntityAccess())
            {
                if (health.ValueRO.HealthAmount <= 0)
                {
                    entityCommandBuffer.DestroyEntity(entity);
                }
            }
        }
    }
}