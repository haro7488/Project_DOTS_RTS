using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class TargetAuthoring : MonoBehaviour
    {
        public GameObject TargetGameObject;

        private class TargetAuthoringBaker : Baker<TargetAuthoring>
        {
            public override void Bake(TargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Target
                {
                    TargetEntity = GetEntity(authoring.TargetGameObject, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct Target : IComponentData
    {
        public Entity TargetEntity;
    }
}