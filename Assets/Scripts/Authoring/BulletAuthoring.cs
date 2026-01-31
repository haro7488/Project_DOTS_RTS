using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class BulletAuthoring : MonoBehaviour
    {
        public float Speed;
        public int DamageAmount;

        private class BulletAuthoringBaker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Bullet
                {
                    Speed = authoring.Speed,
                    DamageAmount = authoring.DamageAmount,
                });
            }
        }
    }

    public struct Bullet : IComponentData
    {
        public float Speed;
        public int DamageAmount;
    }
}