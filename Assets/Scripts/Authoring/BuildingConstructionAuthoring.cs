using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DotsRts
{
    public class BuildingConstructionAuthoring : MonoBehaviour
    {
        private class BuildingConstructionAuthoringBaker : Baker<BuildingConstructionAuthoring>
        {
            public override void Bake(BuildingConstructionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BuildingConstruction());
            }
        }
    }
    
    public struct BuildingConstruction : IComponentData
    {
        public float ConstructionTimer;
        public float ConstructionTimerMax;
        public float3 StartPosition;
        public float3 EndPosition;
        public BuildingType BuildingType;
        public Entity FinalPrefabEntity;
        public Entity VisualEntity;
    }
}