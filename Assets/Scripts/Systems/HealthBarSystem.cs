using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DotsRts.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct HealthBarSystem : ISystem
    {
        // [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var cameraForward = Vector3.zero;
            if (Camera.main != null)
            {
                cameraForward = Camera.main.transform.forward;
            }

            foreach (var (localTransform,
                         healthBar)
                     in SystemAPI.Query<
                         RefRW<LocalTransform>,
                         RefRO<HealthBar>>())
            {
                var parentLocalTransform = SystemAPI.GetComponent<LocalTransform>(healthBar.ValueRO.HealthEntity);
                if (Mathf.Approximately(localTransform.ValueRO.Scale, 1f))
                {
                    localTransform.ValueRW.Rotation = parentLocalTransform.InverseTransformRotation(
                        quaternion.LookRotationSafe(cameraForward, math.up()));
                }

                var health = SystemAPI.GetComponent<Health>(healthBar.ValueRO.HealthEntity);
                if (!health.OnHealthChanged)
                {
                    continue;
                }
                
                var healthNormalized = (float)health.HealthAmount / health.HealthAmountMax;

                localTransform.ValueRW.Scale = Mathf.Approximately(healthNormalized, 1f) ? 0f : 1f;

                var barVisualPostTransformMatrix =
                    SystemAPI.GetComponentRW<PostTransformMatrix>(healthBar.ValueRO.BarVisualEntity);
                barVisualPostTransformMatrix.ValueRW.Value = float4x4.Scale(healthNormalized, 1, 1);
            }
        }
    }
}