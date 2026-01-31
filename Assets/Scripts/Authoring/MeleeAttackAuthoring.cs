using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class MeleeAttackAuthoring : MonoBehaviour
    {
        public float TimerMax;
        public int DamageAmount;
        public float ColliderSize;

        private class MeleeAttackAuthoringBaker : Baker<MeleeAttackAuthoring>
        {
            public override void Bake(MeleeAttackAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MeleeAttack
                {
                    TimerMax = authoring.TimerMax,
                    DamageAmount = authoring.DamageAmount,
                    ColliderSize = authoring.ColliderSize,
                });
            }
        }
    }

    public struct MeleeAttack : IComponentData
    {
        public float Timer;
        public float TimerMax;
        public int DamageAmount;
        public float ColliderSize;
        public bool OnAttacked;
    }
}