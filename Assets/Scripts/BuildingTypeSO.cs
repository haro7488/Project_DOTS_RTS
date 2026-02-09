using System;
using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public enum BuildingType
    {
        None,
        ZombieSpawner,
        Tower,
        Barracks,
        HQ,
        GoldHarvester,
        IronHarvester,
        OilHarvester,
    }

    [CreateAssetMenu]
    public class BuildingTypeSO : ScriptableObject
    {
        public BuildingType BuildingType;
        public Transform Prefab;
        public float BuildingDistanceMin;
        public bool ShowInBuildingPlacementManagerUI;
        public Sprite Sprite;
        public Transform VisualPrefab;
        public ResourceAmount[] BuildCostResourceAmountArray; 

        public bool IsNone()
        {
            return BuildingType == BuildingType.None;
        }

        public Entity GetPrefabEntity(EntitiesReferences entitiesReferences)
        {
            switch (BuildingType)
            {
                default:
                case BuildingType.None:
                case BuildingType.Tower:
                    return entitiesReferences.BuildingTowerPrefabEntity;
                case BuildingType.Barracks:
                    return entitiesReferences.BuildingBarracksPrefabEntity;
                case BuildingType.IronHarvester:
                    return entitiesReferences.BuildingIronHarvesterPrefabEntity;
                case BuildingType.GoldHarvester:
                    return entitiesReferences.BuildingGoldHarvesterPrefabEntity;
                case BuildingType.OilHarvester:
                    return entitiesReferences.BuildingOilHarvesterPrefabEntity;
            }
        }
    }
}