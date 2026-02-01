using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DotsRts
{
    [CreateAssetMenu]
    public class UnitTypeListSO : ScriptableObject
    {
        public List<UnitTypeSO> UnitTypeSOList;

        public UnitTypeSO GetUnitTypeSO(UnitType unitType)
        {
            foreach (var unitTypeSo in UnitTypeSOList)
            {
                if (unitTypeSo.UnitType == unitType)
                {
                    return unitTypeSo;
                }
            }

            Debug.Log("UnitTypeSO not found for UnitType: " + unitType);
            return null;
        }
    }
}