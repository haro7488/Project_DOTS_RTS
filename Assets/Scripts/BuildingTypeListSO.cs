using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DotsRts
{
    [CreateAssetMenu]
    public class BuildingTypeListSO : ScriptableObject
    {
        public List<BuildingTypeSO> BuildingTypeSOList;

        public BuildingTypeSO None;
        
        public BuildingTypeSO GetBuildingTypeSO(BuildingType buildingType)
        {
            foreach (var buildingTypeSo in BuildingTypeSOList)
            {
                if (buildingTypeSo.BuildingType == buildingType)
                {
                    return buildingTypeSo;
                }
            }

            Debug.Log("BuildingTypeSO not found for BuildingType: " + buildingType);
            return null;
        }
    }
}