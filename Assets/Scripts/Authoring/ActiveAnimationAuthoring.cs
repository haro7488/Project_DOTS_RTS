using Unity.Entities;
using UnityEngine;

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