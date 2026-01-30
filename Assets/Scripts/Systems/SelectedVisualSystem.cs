using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DotsRts.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    [UpdateBefore(typeof(ResetEventSystem))]
    public partial struct SelectedVisualSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var selected in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>())
            {
                if (selected.ValueRO.OnSelected)
                {
                    var localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.VisualEntity);
                    localTransform.ValueRW.Scale = selected.ValueRO.ShowScale;
                }

                if (selected.ValueRO.OnDeselected)
                {
                    var localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.VisualEntity);
                    localTransform.ValueRW.Scale = 0f;
                }
            }
        }
    }
}