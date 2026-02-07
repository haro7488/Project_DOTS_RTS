using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class BuildingHQAuthoring : MonoBehaviour
    {
        private class BuildingHQAuthoringBaker : Baker<BuildingHQAuthoring>
        {
            public override void Bake(BuildingHQAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BuildingHQ>(entity);
            }
        }
    }

    public struct BuildingHQ : IComponentData
    {
    }
}