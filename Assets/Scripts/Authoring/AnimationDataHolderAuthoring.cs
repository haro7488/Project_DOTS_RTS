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

                {
                    var blobBuilder = new BlobBuilder(Allocator.Temp);
                    ref var animationData = ref blobBuilder.ConstructRoot<AnimationData>();
                    animationData.FrameTimerMax = authoring.SoldierIdle.FrameTimerMax;
                    animationData.FrameMax = authoring.SoldierIdle.MeshArray.Length;

                    var blobBuilderArray = blobBuilder.Allocate(ref animationData.BatchMeshIdBlobArray,
                        authoring.SoldierIdle.MeshArray.Length);
                    for (int i = 0; i < authoring.SoldierIdle.MeshArray.Length; i++)
                    {
                        var mesh = authoring.SoldierIdle.MeshArray[i];
                        blobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
                    }

                    animationDataHolder.SoldierIdle =
                        blobBuilder.CreateBlobAssetReference<AnimationData>(Allocator.Persistent);
                    blobBuilder.Dispose();
                    AddBlobAsset(ref animationDataHolder.SoldierIdle, out var objectHash);
                }

                {
                    var blobBuilder = new BlobBuilder(Allocator.Temp);
                    ref var animationData = ref blobBuilder.ConstructRoot<AnimationData>();
                    animationData.FrameTimerMax = authoring.SoldierWalk.FrameTimerMax;
                    animationData.FrameMax = authoring.SoldierWalk.MeshArray.Length;

                    var blobBuilderArray = blobBuilder.Allocate(ref animationData.BatchMeshIdBlobArray,
                        authoring.SoldierWalk.MeshArray.Length);
                    for (int i = 0; i < authoring.SoldierWalk.MeshArray.Length; i++)
                    {
                        var mesh = authoring.SoldierWalk.MeshArray[i];
                        blobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
                    }

                    animationDataHolder.SoldierWalk =
                        blobBuilder.CreateBlobAssetReference<AnimationData>(Allocator.Persistent);
                    blobBuilder.Dispose();
                    AddBlobAsset(ref animationDataHolder.SoldierWalk, out var objectHash);
                }

                
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