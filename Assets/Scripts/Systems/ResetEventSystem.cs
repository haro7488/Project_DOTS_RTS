using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace DotsRts.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
    public partial struct ResetEventSystem : ISystem
    {
        private NativeArray<JobHandle> _jobHandleNativeArray;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _jobHandleNativeArray = new NativeArray<JobHandle>(4, Allocator.Persistent);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _jobHandleNativeArray[0] = new ResetShootAttackEventsJob().ScheduleParallel(state.Dependency);
            _jobHandleNativeArray[1] = new ResetHealthEventsJob().ScheduleParallel(state.Dependency);
            _jobHandleNativeArray[2] = new ResetSelectedEventsJob().ScheduleParallel(state.Dependency);
            _jobHandleNativeArray[3] = new ResetMeleeAttackEventsJob().ScheduleParallel(state.Dependency);

            state.Dependency = JobHandle.CombineDependencies(_jobHandleNativeArray);
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