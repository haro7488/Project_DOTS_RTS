using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace DotsRts
{
    public class AnimationDataHolderAuthoring : MonoBehaviour
    {
        public AnimationDataListSO AnimationDataListSO;

        private class AnimationDataHolderAuthoringBaker : Baker<AnimationDataHolderAuthoring>
        {
            public override void Bake(AnimationDataHolderAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var animationDataHolder = new AnimationDataHolder();

                var entitiesGraphicsSystem =
                    World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();

                var blobBuilder = new BlobBuilder(Allocator.Temp);
                ref var animationDataBlobArray = ref blobBuilder.ConstructRoot<BlobArray<AnimationData>>();

                var animationDataBlobBuilderArray = blobBuilder.Allocate(ref animationDataBlobArray,
                    System.Enum.GetValues(typeof(AnimationType)).Length);

                var index = 0;
                foreach (AnimationType animationType in System.Enum.GetValues(typeof(AnimationType)))
                {
                    var animationDataSO = authoring.AnimationDataListSO.GetAnimationDataSO(animationType);
                    if (animationDataSO == null)
                    {
                        continue;
                    }

                    var blobBuilderArray = blobBuilder.Allocate(
                        ref animationDataBlobBuilderArray[index].BatchMeshIdBlobArray,
                        animationDataSO.MeshArray.Length);

                    animationDataBlobBuilderArray[index].FrameTimerMax = animationDataSO.FrameTimerMax;
                    animationDataBlobBuilderArray[index].FrameMax = animationDataSO.MeshArray.Length;

                    for (int i = 0; i < animationDataSO.MeshArray.Length; i++)
                    {
                        var mesh = animationDataSO.MeshArray[i];
                        blobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
                    }

                    index++;
                }

                animationDataHolder.AnimationDataBlobArrayBlobAssetReference =
                    blobBuilder.CreateBlobAssetReference<BlobArray<AnimationData>>(Allocator.Persistent);

                blobBuilder.Dispose();
                AddBlobAsset(ref animationDataHolder.AnimationDataBlobArrayBlobAssetReference, out var objectHash);

                AddComponent(entity, animationDataHolder);
            }
        }
    }

    public struct AnimationDataHolder : IComponentData
    {
        public BlobAssetReference<BlobArray<AnimationData>> AnimationDataBlobArrayBlobAssetReference;
    }

    public struct AnimationData
    {
        public float FrameTimerMax;
        public int FrameMax;
        public BlobArray<BatchMeshID> BatchMeshIdBlobArray;
    }
}