using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace DotsRts
{
    public class ActiveAnimationAuthoring : MonoBehaviour
    {
        public AnimationType NextAnimationType;

        private class ActiveAnimationAuthoringBaker : Baker<ActiveAnimationAuthoring>
        {
            public override void Bake(ActiveAnimationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                var entitiesGraphicsSystem =
                    World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();

                AddComponent(entity, new ActiveAnimation
                {
                    NextAnimationType = authoring.NextAnimationType,
                });
            }
        }
    }

    public struct ActiveAnimation : IComponentData
    {
        public int Frame;
        public float FrameTimer;
        public AnimationType ActiveAnimationType;
        public AnimationType NextAnimationType;
    }
}