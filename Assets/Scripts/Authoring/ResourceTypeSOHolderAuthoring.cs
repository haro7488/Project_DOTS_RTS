using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class ResourceTypeSOHolderAuthoring : MonoBehaviour
    {
        public ResourceType resourceType;

        private class ResourceTypeSOHolderAuthoringBaker : Baker<ResourceTypeSOHolderAuthoring>
        {
            public override void Bake(ResourceTypeSOHolderAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ResourceTypeSOHolder
                {
                    resourceType = authoring.resourceType
                });
            }
        }
    }

    public struct ResourceTypeSOHolder : IComponentData
    {
        public ResourceType resourceType;
    }
}