using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class VisualUnderFogOfWarAuthoring : MonoBehaviour
    {
        public GameObject ParentGameObject;
        public float SphereCastSize;

        private class VisualUnderFogOfWarAuthoringBaker : Baker<VisualUnderFogOfWarAuthoring>
        {
            public override void Bake(VisualUnderFogOfWarAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new VisualUnderFogOfWar
                {
                    IsVisible = true,
                    ParentEntity = GetEntity(authoring.ParentGameObject, TransformUsageFlags.Dynamic),
                    SphereCastSize = authoring.SphereCastSize
                });
            }
        }
    }

    public struct VisualUnderFogOfWar : IComponentData
    {
        public bool IsVisible;
        public Entity ParentEntity;
        public float SphereCastSize;
    }
}