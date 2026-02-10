using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class EntitiesReferencesAuthoring : MonoBehaviour
    {
        public GameObject BulletPrefabGameObject;
        public GameObject ZombiePrefabGameObject;
        public GameObject ShootLightPrefabGameObject;
        public GameObject ScoutPrefabGameObject;
        public GameObject SoldierPrefabGameObject;
        
        public GameObject BuildingTowerPrefabGameObject;
        public GameObject BuildingBarracksPrefabGameObject;
        public GameObject BuildingIronHarvesterPrefabGameObject;
        public GameObject BuildingGoldHarvesterPrefabGameObject;
        public GameObject BuildingOilHarvesterPrefabGameObject;
        
        public GameObject BuildingTowerVisualPrefabGameObject;
        public GameObject BuildingBarracksVisualPrefabGameObject;
        public GameObject BuildingIronHarvesterVisualPrefabGameObject;
        public GameObject BuildingGoldHarvesterVisualPrefabGameObject;
        public GameObject BuildingOilHarvesterVisualPrefabGameObject;
        
        public GameObject BuildingConstructionPrefabGameObject;

        private class EntityReferencesAuthoringBaker : Baker<EntitiesReferencesAuthoring>
        {
            public override void Bake(EntitiesReferencesAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EntitiesReferences
                {
                    BulletPrefabEntity = GetEntity(authoring.BulletPrefabGameObject, TransformUsageFlags.Dynamic),
                    ZombiePrefabEntity = GetEntity(authoring.ZombiePrefabGameObject, TransformUsageFlags.Dynamic),
                    ShootLightPrefabEntity = GetEntity(authoring.ShootLightPrefabGameObject, TransformUsageFlags.Dynamic),
                    ScoutPrefabEntity = GetEntity(authoring.ScoutPrefabGameObject, TransformUsageFlags.Dynamic),
                    SoldierPrefabEntity = GetEntity(authoring.SoldierPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingTowerPrefabEntity = GetEntity(authoring.BuildingTowerPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingBarracksPrefabEntity = GetEntity(authoring.BuildingBarracksPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingIronHarvesterPrefabEntity = GetEntity(authoring.BuildingIronHarvesterPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingGoldHarvesterPrefabEntity = GetEntity(authoring.BuildingGoldHarvesterPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingOilHarvesterPrefabEntity = GetEntity(authoring.BuildingOilHarvesterPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingTowerVisualPrefabEntity = GetEntity(authoring.BuildingTowerVisualPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingBarracksVisualPrefabEntity = GetEntity(authoring.BuildingBarracksVisualPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingIronHarvesterVisualPrefabEntity = GetEntity(authoring.BuildingIronHarvesterVisualPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingGoldHarvesterVisualPrefabEntity = GetEntity(authoring.BuildingGoldHarvesterVisualPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingOilHarvesterVisualPrefabEntity = GetEntity(authoring.BuildingOilHarvesterVisualPrefabGameObject, TransformUsageFlags.Dynamic),
                    BuildingConstructionPrefabEntity = GetEntity(authoring.BuildingConstructionPrefabGameObject, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct EntitiesReferences : IComponentData
    {
        public Entity BulletPrefabEntity;
        public Entity ZombiePrefabEntity;
        public Entity ShootLightPrefabEntity;
        public Entity ScoutPrefabEntity;
        public Entity SoldierPrefabEntity;

        public Entity BuildingTowerPrefabEntity;
        public Entity BuildingBarracksPrefabEntity;
        public Entity BuildingIronHarvesterPrefabEntity;
        public Entity BuildingGoldHarvesterPrefabEntity;
        public Entity BuildingOilHarvesterPrefabEntity;
        
        public Entity BuildingTowerVisualPrefabEntity;
        public Entity BuildingBarracksVisualPrefabEntity;
        public Entity BuildingIronHarvesterVisualPrefabEntity;
        public Entity BuildingGoldHarvesterVisualPrefabEntity;
        public Entity BuildingOilHarvesterVisualPrefabEntity;
        
        public Entity BuildingConstructionPrefabEntity;
    }
}