using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class TargetAuthoring : MonoBehaviour
    {
        private class TargetAuthoringBaker : Baker<TargetAuthoring>
        {
            public override void Bake(TargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Target>(entity);
            }
        }
    }

    public struct Target : IComponentData
    {
        public Entity TargetEntity;
    }
}