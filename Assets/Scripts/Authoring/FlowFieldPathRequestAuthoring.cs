using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DotsRts
{
    public class FlowFieldPathRequestAuthoring : MonoBehaviour
    {
        private class FlowFieldPathRequestAuthoringBaker : Baker<FlowFieldPathRequestAuthoring>
        {
            public override void Bake(FlowFieldPathRequestAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new FlowFieldPathRequest());
                SetComponentEnabled<FlowFieldPathRequest>(entity, false);
            }
        }
    }

    public struct FlowFieldPathRequest : IComponentData, IEnableableComponent
    {
        public float3 TargetPosition;
    }
}