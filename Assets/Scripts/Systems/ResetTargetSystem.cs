using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace DotsRts.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial struct ResetTargetSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _localTransformComponentLookup;
        private EntityStorageInfoLookup _entityStorageInfoLookup;

        public void OnCreate(ref SystemState state)
        {
            _localTransformComponentLookup = state.GetComponentLookup<LocalTransform>(true);
            _entityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _localTransformComponentLookup.Update(ref state);
            _entityStorageInfoLookup.Update(ref state);
            var resetTargetJob = new ResetTargetJob
            {
                LocalTransformComponentLookup = _localTransformComponentLookup,
                EntityStorageInfoLookup = _entityStorageInfoLookup
            };
            resetTargetJob.ScheduleParallel();
            
            var resetTargetOverrideJob = new ResetTargetOverrideJob
            {
                LocalTransformComponentLookup = _localTransformComponentLookup,
                EntityStorageInfoLookup = _entityStorageInfoLookup
            };
            resetTargetOverrideJob.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct ResetTargetJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformComponentLookup;
            [ReadOnly] public EntityStorageInfoLookup EntityStorageInfoLookup;

            public void Execute(ref Target target)
            {
                if (target.TargetEntity != Entity.Null)
                {
                    if (!EntityStorageInfoLookup.Exists(target.TargetEntity) ||
                        !LocalTransformComponentLookup.HasComponent(target.TargetEntity))
                    {
                        target.TargetEntity = Entity.Null;
                    }
                }
            }
        }
        
        [BurstCompile]
        public partial struct ResetTargetOverrideJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformComponentLookup;
            [ReadOnly] public EntityStorageInfoLookup EntityStorageInfoLookup;

            public void Execute(ref TargetOverride targetOverride)
            {
                if (targetOverride.TargetEntity != Entity.Null)
                {
                    if (!EntityStorageInfoLookup.Exists(targetOverride.TargetEntity) ||
                        !LocalTransformComponentLookup.HasComponent(targetOverride.TargetEntity))
                    {
                        targetOverride.TargetEntity = Entity.Null;
                    }
                }
            }
        }
    }
}