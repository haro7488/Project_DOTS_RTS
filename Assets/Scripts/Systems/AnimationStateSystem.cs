using Unity.Burst;
using Unity.Entities;
using Debug = UnityEngine.Debug;

namespace DotsRts.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup), OrderFirst = true)]
    public partial struct AnimationStateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (animatedMesh,
                         unitMover,
                         unitAnimation)
                     in SystemAPI.Query<
                         RefRW<AnimatedMesh>,
                         RefRO<UnitMover>,
                         RefRO<UnitAnimation>>())
            {
                var activeAnimation = SystemAPI.GetComponentRW<ActiveAnimation>(animatedMesh.ValueRO.MeshEntity);
                if (unitMover.ValueRO.IsMoving)
                {
                    activeAnimation.ValueRW.NextAnimationType = unitAnimation.ValueRO.WalkAnimationType;
                }
                else
                {
                    activeAnimation.ValueRW.NextAnimationType = unitAnimation.ValueRO.IdleAnimationType;
                }
            }

            foreach (var (animatedMesh,
                         shootAttack,
                         unitMover,
                         target,
                         unitAnimation)
                     in SystemAPI.Query<
                         RefRW<AnimatedMesh>,
                         RefRO<ShootAttack>,
                         RefRO<UnitMover>,
                         RefRO<Target>,
                         RefRO<UnitAnimation>>())
            {
                if (!unitMover.ValueRO.IsMoving && target.ValueRO.TargetEntity != Entity.Null)
                {
                    var activeAnimation = SystemAPI.GetComponentRW<ActiveAnimation>(animatedMesh.ValueRO.MeshEntity);
                    activeAnimation.ValueRW.NextAnimationType = unitAnimation.ValueRO.AimAnimationType;
                }

                if (shootAttack.ValueRO.OnShoot.IsTriggered)
                {
                    var activeAnimation = SystemAPI.GetComponentRW<ActiveAnimation>(animatedMesh.ValueRO.MeshEntity);
                    activeAnimation.ValueRW.NextAnimationType = unitAnimation.ValueRO.ShootAnimationType;
                }
            }

            foreach (var (animatedMesh,
                         meleeAttack,
                         unitAnimation)
                     in SystemAPI.Query<
                         RefRW<AnimatedMesh>,
                         RefRO<MeleeAttack>,
                         RefRO<UnitAnimation>>())
            {
                if (meleeAttack.ValueRO.OnAttacked)
                {
                    var activeAnimation = SystemAPI.GetComponentRW<ActiveAnimation>(animatedMesh.ValueRO.MeshEntity);
                    activeAnimation.ValueRW.NextAnimationType = unitAnimation.ValueRO.MeleeAnimationType;
                }
            }
        }
    }
}