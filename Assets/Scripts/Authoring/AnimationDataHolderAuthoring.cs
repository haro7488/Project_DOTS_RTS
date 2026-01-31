using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace DotsRts
{
    public class AnimationDataHolderAuthoring : MonoBehaviour
    {
        public AnimationDataSO SoldierIdle;
        public AnimationDataSO SoldierWalk;

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

                var animationDataBlobBuilderArray = blobBuilder.Allocate(ref animationDataBlobArray, 2);

                {
                    var blobBuilderArray = blobBuilder.Allocate(
                        ref animationDataBlobBuilderArray[0].BatchMeshIdBlobArray,
                        authoring.SoldierIdle.MeshArray.Length);

                    animationDataBlobBuilderArray[0].FrameTimerMax = authoring.SoldierIdle.FrameTimerMax;
                    animationDataBlobBuilderArray[0].FrameMax = authoring.SoldierIdle.MeshArray.Length;

                    for (int i = 0; i < authoring.SoldierIdle.MeshArray.Length; i++)
                    {
                        var mesh = authoring.SoldierIdle.MeshArray[i];
                        blobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
                    }
                }
                
                {
                    var blobBuilderArray = blobBuilder.Allocate(
                        ref animationDataBlobBuilderArray[1].BatchMeshIdBlobArray,
                        authoring.SoldierWalk.MeshArray.Length);

                    animationDataBlobBuilderArray[1].FrameTimerMax = authoring.SoldierWalk.FrameTimerMax;
                    animationDataBlobBuilderArray[1].FrameMax = authoring.SoldierWalk.MeshArray.Length;

                    for (int i = 0; i < authoring.SoldierWalk.MeshArray.Length; i++)
                    {
                        var mesh = authoring.SoldierWalk.MeshArray[i];
                        blobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
                    }
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