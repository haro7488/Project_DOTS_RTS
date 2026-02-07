using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DotsRts.Systems
{
    public partial struct EnemyAttackHQSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var hqEntity = SystemAPI.GetSingletonEntity<BuildingHQ>();
            var hqPosition = SystemAPI.GetComponent<LocalTransform>(hqEntity).Position;

            foreach (var (enemyAttackHQ,
                         unitMover,
                         target)
                     in SystemAPI.Query<
                         RefRO<EnemyAttackHQ>,
                         RefRW<UnitMover>,
                         RefRO<Target>>().WithDisabled<MoveOverride>())
            {
                if (target.ValueRO.TargetEntity != Entity.Null)
                {
                    continue;
                }

                unitMover.ValueRW.TargetPosition = hqPosition;
            }
        }
    }
}