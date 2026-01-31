using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using UnityEditor.Build.Player;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DotsRts.Systems
{
    public partial struct ShootAttackSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (shootAttack,
                         target)
                     in SystemAPI.Query<
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
                
                var targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.TargetEntity);
                var damageAmount = 1;
                targetHealth.ValueRW.HealthAmount -= damageAmount;
            }
        }
    }
}