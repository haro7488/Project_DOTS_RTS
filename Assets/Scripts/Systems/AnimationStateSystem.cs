using Unity.Burst;
using Unity.Entities;

namespace DotsRts.Systems
{
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
        }
    }
}