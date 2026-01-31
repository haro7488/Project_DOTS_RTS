using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DotsRts.Systems
{
    public partial struct SetupUnitMoverDefaultPositionSystem : ISystem
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
                         unitMover,
                         setupUnitMoverDefaultPosition,
                         entity)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<UnitMover>,
                         RefRO<SetupUnitMoverDefaultPosition>>().WithEntityAccess())
            {
                unitMover.ValueRW.TargetPosition = localTransform.ValueRO.Position;
                entityCommandBuffer.RemoveComponent<SetupUnitMoverDefaultPosition>(entity);
            }
        }
    }
}