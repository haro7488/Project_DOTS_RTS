using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DotsRts.Systems
{
    public partial struct SelectedVisualSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var selected in SystemAPI.Query<RefRO<Selected>>().WithDisabled<Selected>())
            {
                var localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.VisualEntity);
                localTransform.ValueRW.Scale = 0f;
            }
            
            foreach (var selected in SystemAPI.Query<RefRO<Selected>>())
            {
                var localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.VisualEntity);
                localTransform.ValueRW.Scale = selected.ValueRO.ShowScale;
            }
        }
    }
}