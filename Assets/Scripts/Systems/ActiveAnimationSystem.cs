using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

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
                if (Input.GetKeyDown(KeyCode.T))
                {
                    activeAnimation.ValueRW.ActiveAnimationIndex = 0;
                }

                if (Input.GetKeyDown(KeyCode.Y))
                {
                    activeAnimation.ValueRW.ActiveAnimationIndex = 1;
                }


                ref var animationData = ref animationDataHolder.AnimationDataBlobArrayBlobAssetReference.Value[
                    activeAnimation.ValueRW.ActiveAnimationIndex];

                activeAnimation.ValueRW.FrameTimer += SystemAPI.Time.DeltaTime;
                if (activeAnimation.ValueRW.FrameTimer > animationData.FrameTimerMax)
                {
                    activeAnimation.ValueRW.FrameTimer -= animationData.FrameTimerMax;
                    activeAnimation.ValueRW.Frame = (activeAnimation.ValueRW.Frame + 1) % animationData.FrameMax;

                    materialMeshInfo.ValueRW.MeshID = animationData.BatchMeshIdBlobArray[activeAnimation.ValueRW.Frame];
                }
            }
        }
    }
}