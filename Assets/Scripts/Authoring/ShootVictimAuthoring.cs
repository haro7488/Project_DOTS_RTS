using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DotsRts
{
    public class ShootVictimAuthoring : MonoBehaviour
    {
        public Transform HitPositionTransform;

        private class ShootVictimAuthoringBaker : Baker<ShootVictimAuthoring>
        {
            public override void Bake(ShootVictimAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ShootVictim
                {
                    HitLocalPosition = authoring.HitPositionTransform.localPosition
                });
            }
        }
    }

    public struct ShootVictim : IComponentData
    {
        public float3 HitLocalPosition;
    }
}