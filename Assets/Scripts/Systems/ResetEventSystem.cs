using Unity.Burst;
using Unity.Entities;

namespace DotsRts.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
    public partial struct ResetEventSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ResetShootAttackEventsJob().ScheduleParallel();
            new ResetHealthEventsJob().ScheduleParallel();
            new ResetSelectedEventsJob().ScheduleParallel();
            new ResetMeleeAttackEventsJob().ScheduleParallel();

            // foreach (var selected in SystemAPI.Query<RefRW<Selected>>().WithPresent<Selected>())
            // {
            //     selected.ValueRW.OnSelected = false;
            //     selected.ValueRW.OnDeselected = false;
            // }
            //
            // foreach (var health in SystemAPI.Query<RefRW<Health>>())
            // {
            //     health.ValueRW.OnHealthChanged = false;
            // }
            //
            // foreach (var shootAttack in SystemAPI.Query<RefRW<ShootAttack>>())
            // {
            //     shootAttack.ValueRW.OnShoot.IsTriggered = false;
            // }
        }
    }

    [BurstCompile]
    public partial struct ResetShootAttackEventsJob : IJobEntity
    {
        public void Execute(ref ShootAttack shootAttack)
        {
            shootAttack.OnShoot.IsTriggered = false;
        }
    }

    [BurstCompile]
    public partial struct ResetHealthEventsJob : IJobEntity
    {
        public void Execute(ref Health health)
        {
            health.OnHealthChanged = false;
        }
    }

    [BurstCompile]
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    public partial struct ResetSelectedEventsJob : IJobEntity
    {
        public void Execute(ref Selected selected)
        {
            selected.OnSelected = false;
            selected.OnDeselected = false;
        }
    }

    [BurstCompile]
    public partial struct ResetMeleeAttackEventsJob : IJobEntity
    {
        public void Execute(ref MeleeAttack meleeAttack)
        {
            meleeAttack.OnAttacked = false;
        }
    }
}