using UnityEngine;

namespace DotsRts
{
    [CreateAssetMenu]
    public class BuildingResourceHarvesterTypeSO : BuildingTypeSO
    {
        public ResourceType HarvestableResourceType;
        public float HarvestDistance;
    }
}