using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class BuildingTypeSOHolderAuthoring : MonoBehaviour
    {
        public BuildingType BuildingType;

        private class BuildingTypeSOHolderAuthoringBaker : Baker<BuildingTypeSOHolderAuthoring>
        {
            public override void Bake(BuildingTypeSOHolderAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BuildingTypeSOHolder
                {
                    BuildingType = authoring.BuildingType,
                });
            }
        }
    }

    public struct BuildingTypeSOHolder : IComponentData
    {
        public BuildingType BuildingType;
    }
}