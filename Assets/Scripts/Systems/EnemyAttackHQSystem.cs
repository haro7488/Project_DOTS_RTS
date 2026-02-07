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
                         targetPositionPathQueued,
                         targetPositionPathQueuedEnabled,
                         target)
                     in SystemAPI.Query<
                         RefRO<EnemyAttackHQ>,
                         RefRW<TargetPositionPathQueued>,
                         EnabledRefRW<TargetPositionPathQueued>,
                         RefRO<Target>>().WithDisabled<MoveOverride>().WithPresent<TargetPositionPathQueued>())
            {
                if (target.ValueRO.TargetEntity != Entity.Null)
                {
                    continue;
                }

                targetPositionPathQueued.ValueRW.TargetPosition = hqPosition;
                targetPositionPathQueuedEnabled.ValueRW = true;
            }
        }
    }
}