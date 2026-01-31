using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEditor.Build.Player;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DotsRts.Systems
{
    public partial struct ShootAttackSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EntitiesReferences>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

            foreach (var (localTransform,
                         shootAttack,
                         target)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<ShootAttack>,
                         RefRO<Target>>())
            {
                if (target.ValueRO.TargetEntity == Entity.Null)
                {
                    continue;
                }

                shootAttack.ValueRW.Timer -= SystemAPI.Time.DeltaTime;
                if (shootAttack.ValueRO.Timer > 0f)
                {
                    continue;
                }

                shootAttack.ValueRW.Timer = shootAttack.ValueRO.TimerMax;

                var bulletEntity = state.EntityManager.Instantiate(entitiesReferences.BulletPrefabEntity);
                SystemAPI.SetComponent(bulletEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));

                var bulletBullet = SystemAPI.GetComponentRW<Bullet>(bulletEntity);
                bulletBullet.ValueRW.DamageAmount = shootAttack.ValueRO.DamageAmount;

                var bulletTarget = SystemAPI.GetComponentRW<Target>(bulletEntity);
                bulletTarget.ValueRW.TargetEntity = target.ValueRO.TargetEntity;
            }
        }
    }
}