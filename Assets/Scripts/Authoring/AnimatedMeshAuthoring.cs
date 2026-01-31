using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class AnimatedMeshAuthoring : MonoBehaviour
    {
        public GameObject MeshGameObject;

        private class AnimatedMeshAuthoringBaker : Baker<AnimatedMeshAuthoring>
        {
            public override void Bake(AnimatedMeshAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AnimatedMesh
                {
                    MeshEntity = GetEntity(authoring.MeshGameObject, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct AnimatedMesh : IComponentData
    {
        public Entity MeshEntity;
    }
}