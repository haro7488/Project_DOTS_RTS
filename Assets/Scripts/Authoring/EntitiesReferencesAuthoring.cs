using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class EntitiesReferencesAuthoring : MonoBehaviour
    {
        public GameObject BulletPrefabGameObject;
        public GameObject ZombiePrefabGameObject;
        public GameObject ShootLightPrefabGameObject;

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
                });
            }
        }
    }

    public struct EntitiesReferences : IComponentData
    {
        public Entity BulletPrefabEntity;
        public Entity ZombiePrefabEntity;
        public Entity ShootLightPrefabEntity;
    }
}