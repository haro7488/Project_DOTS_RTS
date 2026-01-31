using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class UnitAnimationAuthoring : MonoBehaviour
    {
        public AnimationType IdleAnimationType;
        public AnimationType WalkAnimationType;
        public AnimationType ShootAnimationType;
        public AnimationType AimAnimationType;
        public AnimationType MeleeAnimationType;

        private class UnitAnimationAuthoringBaker : Baker<UnitAnimationAuthoring>
        {
            public override void Bake(UnitAnimationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new UnitAnimation
                {
                    IdleAnimationType = authoring.IdleAnimationType,
                    WalkAnimationType = authoring.WalkAnimationType,
                    ShootAnimationType = authoring.ShootAnimationType,
                    AimAnimationType = authoring.AimAnimationType,
                    MeleeAnimationType = authoring.MeleeAnimationType,
                });
            }
        }
    }

    public struct UnitAnimation : IComponentData
    {
        public AnimationType IdleAnimationType;
        public AnimationType WalkAnimationType;
        public AnimationType ShootAnimationType;
        public AnimationType AimAnimationType;
        public AnimationType MeleeAnimationType;
    }
}