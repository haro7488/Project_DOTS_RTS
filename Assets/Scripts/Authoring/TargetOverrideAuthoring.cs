using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class TargetOverrideAuthoring : MonoBehaviour
    {
        private class TargetOverrideAuthoringBaker : Baker<TargetOverrideAuthoring>
        {
            public override void Bake(TargetOverrideAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<TargetOverride>(entity);
            }
        }
    }
    
    public struct TargetOverride : IComponentData
    {
        public Entity TargetEntity;
    }
}