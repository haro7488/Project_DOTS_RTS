using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class BuildingHarvesterAuthoring : MonoBehaviour
    {
        public float HearvestTimerMax;
        public ResourceType ResourceType;

        private class BuildingHarvesterAuthoringBaker : Baker<BuildingHarvesterAuthoring>
        {
            public override void Bake(BuildingHarvesterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BuildingHarvester
                {
                    HarvestTimerMax = authoring.HearvestTimerMax,
                    ResourceType = authoring.ResourceType
                });
            }
        }
    }

    public struct BuildingHarvester : IComponentData
    {
        public float HarvestTimer;
        public float HarvestTimerMax;
        public ResourceType ResourceType;
    }
}