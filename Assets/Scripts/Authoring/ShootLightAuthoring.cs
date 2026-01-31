using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class ShootLightAuthoring : MonoBehaviour
    {
        public float Timer;

        private class ShootLightAuthoringBaker : Baker<ShootLightAuthoring>
        {
            public override void Bake(ShootLightAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ShootLight
                {
                    Timer = authoring.Timer,
                });
            }
        }
    }

    public struct ShootLight : IComponentData
    {
        public float Timer;
    }
}