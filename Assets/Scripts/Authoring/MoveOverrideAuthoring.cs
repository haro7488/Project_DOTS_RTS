using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DotsRts
{
    public class MoveOverrideAuthoring : MonoBehaviour
    {
        private class MoveOverrideAuthoringBaker : Baker<MoveOverrideAuthoring>
        {
            public override void Bake(MoveOverrideAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MoveOverride());
                SetComponentEnabled<MoveOverride>(entity, false);
            }
        }
    }

    public struct MoveOverride : IComponentData, IEnableableComponent
    {
        public float3 TargetPosition;
    }
}