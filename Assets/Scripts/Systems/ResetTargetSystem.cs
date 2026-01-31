using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DotsRts.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial struct ResetTargetSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var target in SystemAPI.Query<RefRW<Target>>())
            {
                if (target.ValueRO.TargetEntity != Entity.Null)
                {
                    if (!SystemAPI.Exists(target.ValueRO.TargetEntity) ||
                        !SystemAPI.HasComponent<LocalTransform>(target.ValueRO.TargetEntity))
                    {
                        target.ValueRW.TargetEntity = Entity.Null;
                    }
                }
            }
        }
    }
}