using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DotsRts
{
    [CreateAssetMenu]
    public class ResourceTypeListSO : ScriptableObject
    {
        public List<ResourceTypeSO> ResourceTypeSOList;

        public ResourceTypeSO GetResourceTypeSO(ResourceType resourceType)
        {
            foreach (var resourceTypeSo in ResourceTypeSOList)
            {
                if (resourceTypeSo.ResourceType == resourceType)
                {
                    return resourceTypeSo;
                }
            }

            Debug.Log("ResourceTypeSO not found for resource type " + resourceType);
            return null;
        }
    }
}