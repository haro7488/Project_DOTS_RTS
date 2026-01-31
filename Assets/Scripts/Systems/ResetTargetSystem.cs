using Unity.Burst;
using Unity.Entities;

namespace DotsRts.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct ResetTargetSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var target in SystemAPI.Query<RefRW<Target>>())
            {
                if (!SystemAPI.Exists(target.ValueRO.TargetEntity))
                {
                    target.ValueRW.TargetEntity = Entity.Null;
                }
            }
        }
    }
}