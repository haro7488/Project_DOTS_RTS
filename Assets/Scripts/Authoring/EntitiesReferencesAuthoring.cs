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
    }
}