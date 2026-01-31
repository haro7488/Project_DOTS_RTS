using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class ShootAttackAuthoring : MonoBehaviour
    {
        public float TimerMax;
        public int DamageAmount = 1;

        private class ShootAttackAuthoringBaker : Baker<ShootAttackAuthoring>
        {
            public override void Bake(ShootAttackAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ShootAttack
                {
                    TimerMax = authoring.TimerMax,
                    DamageAmount = authoring.DamageAmount,
                });
            }
        }
    }

    public struct ShootAttack : IComponentData
    {
        public float Timer;
        public float TimerMax;
        public int DamageAmount;
    }
}