using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class UnitAnimationAuthoring : MonoBehaviour
    {
        public AnimationType IdleAnimationType;
        public AnimationType WalkAnimationType;

        private class UnitAnimationAuthoringBaker : Baker<UnitAnimationAuthoring>
        {
            public override void Bake(UnitAnimationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new UnitAnimation
                {
                    IdleAnimationType = authoring.IdleAnimationType,
                    WalkAnimationType = authoring.WalkAnimationType,
                });
            }
        }
    }

    public struct UnitAnimation : IComponentData
    {
        public AnimationType IdleAnimationType;
        public AnimationType WalkAnimationType;
    }
}