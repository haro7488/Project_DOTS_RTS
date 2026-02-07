using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DotsRts
{
    public class TargetPositionPathQueuedAuthoring : MonoBehaviour
    {
        private class TargetPositionPathQueuedAuthoringBaker : Baker<TargetPositionPathQueuedAuthoring>
        {
            public override void Bake(TargetPositionPathQueuedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<TargetPositionPathQueued>(entity);
                SetComponentEnabled<TargetPositionPathQueued>(entity, false);
            }
        }
    }

    public struct TargetPositionPathQueued : IComponentData, IEnableableComponent
    {
        public float3 TargetPosition;
    }
}