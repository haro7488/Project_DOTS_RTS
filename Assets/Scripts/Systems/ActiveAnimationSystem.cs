using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace DotsRts.Systems
{
    public partial struct ActiveAnimationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AnimationDataHolder>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var animationDataHolder = SystemAPI.GetSingleton<AnimationDataHolder>();

            foreach (var (activeAnimation,
                         materialMeshInfo)
                     in SystemAPI.Query<
                         RefRW<ActiveAnimation>,
                         RefRW<MaterialMeshInfo>>())
            {
                if (!activeAnimation.ValueRO.AnimationDataBlobAssetReference.IsCreated)
                {
                    activeAnimation.ValueRW.AnimationDataBlobAssetReference = animationDataHolder.SoldierWalk;
                }

                activeAnimation.ValueRW.FrameTimer += SystemAPI.Time.DeltaTime;
                if (activeAnimation.ValueRW.FrameTimer >
                    activeAnimation.ValueRO.AnimationDataBlobAssetReference.Value.FrameTimerMax)
                {
                    activeAnimation.ValueRW.FrameTimer -=
                        activeAnimation.ValueRO.AnimationDataBlobAssetReference.Value.FrameTimerMax;
                    activeAnimation.ValueRW.Frame =
                        (activeAnimation.ValueRW.Frame + 1) %
                        activeAnimation.ValueRO.AnimationDataBlobAssetReference.Value.FrameMax;

                    materialMeshInfo.ValueRW.MeshID = activeAnimation.ValueRO.AnimationDataBlobAssetReference.Value
                        .BatchMeshIdBlobArray[activeAnimation.ValueRW.Frame];
                }
            }
        }
    }
}