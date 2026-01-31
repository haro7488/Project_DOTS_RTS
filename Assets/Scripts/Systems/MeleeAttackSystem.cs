using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Debug = UnityEngine.Debug;

namespace DotsRts.Systems
{
    [UpdateAfter(typeof(RandomWalkingSystem))]
    public partial struct MeleeAttackSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var collisionWorld = physicsWorldSingleton.CollisionWorld;
            var raycastHitList = new NativeList<RaycastHit>(Allocator.Temp);

            foreach (var (localTransform,
                         meleeAttack,
                         target,
                         unitMover)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<MeleeAttack>,
                         RefRO<Target>,
                         RefRW<UnitMover>>())
            {
                if (target.ValueRO.TargetEntity == Entity.Null)
                {
                    continue;
                }

                var targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.TargetEntity);
                var meleeAttackDistanceSq = 2f;
                var isCloseEnoughToAttack =
                    math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position) <
                    meleeAttackDistanceSq;
                
                var isTouchingTarget = false;
                if (!isCloseEnoughToAttack)
                {
                    var dirToTarget = targetLocalTransform.Position - localTransform.ValueRO.Position;
                    dirToTarget = math.normalize(dirToTarget);

                    var distanceExtraToTestRaycast = .4f;
                    var raycastInput = new RaycastInput
                    {
                        Start = localTransform.ValueRO.Position,
                        End = localTransform.ValueRO.Position +
                              dirToTarget * (meleeAttack.ValueRO.ColliderSize + distanceExtraToTestRaycast),
                        Filter = CollisionFilter.Default
                    };
                    raycastHitList.Clear();
                    if (collisionWorld.CastRay(raycastInput, ref raycastHitList))
                    {
                        foreach (var raycastHit in raycastHitList)
                        {
                            if (raycastHit.Entity == target.ValueRO.TargetEntity)
                            {
                                isTouchingTarget = true;
                                break;
                            }
                        }
                    }
                }

                if (!isCloseEnoughToAttack && !isTouchingTarget)
                {
                    unitMover.ValueRW.TargetPosition = targetLocalTransform.Position;
                }
                else
                {
                    unitMover.ValueRW.TargetPosition = localTransform.ValueRO.Position;
                    meleeAttack.ValueRW.Timer -= SystemAPI.Time.DeltaTime;
                    if (meleeAttack.ValueRO.Timer > 0f)
                    {
                        continue;
                    }

                    meleeAttack.ValueRW.Timer = meleeAttack.ValueRO.TimerMax;

                    var targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.TargetEntity);
                    targetHealth.ValueRW.HealthAmount -= meleeAttack.ValueRO.DamageAmount;
                    targetHealth.ValueRW.OnHealthChanged = true;
                }
            }
        }
    }
}