using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class ShootAttackAuthoring : MonoBehaviour
    {
        public float TimerMax;
        
        private class ShootAttackAuthoringBaker : Baker<ShootAttackAuthoring>
        {
            public override void Bake(ShootAttackAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ShootAttack
                {
                    TimerMax = authoring.TimerMax,
                });
            }
        }
    }

    public struct ShootAttack : IComponentData
    {
        public float Timer;
        public float TimerMax;
    }
}