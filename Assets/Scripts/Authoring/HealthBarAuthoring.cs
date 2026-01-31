using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class HealthBarAuthoring : MonoBehaviour
    {
        public GameObject BarVisualGameObject;
        public GameObject HealthGameObject;

        private class HealthBarAuthoringBaker : Baker<HealthBarAuthoring>
        {
            public override void Bake(HealthBarAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new HealthBar
                {
                    BarVisualEntity = GetEntity(authoring.BarVisualGameObject, TransformUsageFlags.NonUniformScale),
                    HealthEntity = GetEntity(authoring.HealthGameObject, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct HealthBar : IComponentData
    {
        public Entity BarVisualEntity;
        public Entity HealthEntity;
    }
}