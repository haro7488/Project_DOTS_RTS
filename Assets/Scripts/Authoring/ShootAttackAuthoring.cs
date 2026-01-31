using DotsRts.Systems;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DotsRts
{
    public class ShootAttackAuthoring : MonoBehaviour
    {
        public float TimerMax;
        public int DamageAmount = 1;
        public float AttackDistance = 10f;
        public Transform BulletSpawnPositionTransform;

        private class ShootAttackAuthoringBaker : Baker<ShootAttackAuthoring>
        {
            public override void Bake(ShootAttackAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ShootAttack
                {
                    TimerMax = authoring.TimerMax,
                    DamageAmount = authoring.DamageAmount,
                    AttackDistance = authoring.AttackDistance,
                    BulletSpawnLocalPosition = authoring.BulletSpawnPositionTransform.localPosition
                });
            }
        }
    }

    public struct ShootAttack : IComponentData
    {
        public float Timer;
        public float TimerMax;
        public int DamageAmount;
        public float AttackDistance;
        public float3 BulletSpawnLocalPosition;
        public OnShootEvent OnShoot;

        public struct OnShootEvent
        {
            public bool IsTriggered;
            public float3 ShootFromPosition;
        }
    }
}