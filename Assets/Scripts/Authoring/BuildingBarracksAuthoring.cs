using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DotsRts
{
    public class BuildingBarracksAuthoring : MonoBehaviour
    {
        public float ProgressMax;

        private class BuildingBarracksAuthoringBaker : Baker<BuildingBarracksAuthoring>
        {
            public override void Bake(BuildingBarracksAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BuildingBarracks
                {
                    ProgressMax = authoring.ProgressMax,
                    RallyPositionOffset = new float3(10, 0, 0)
                });

                AddBuffer<SpawnUnitTypeBuffer>(entity);

                AddComponent(entity, new BuildingBarracksUnitEnqueue());
                SetComponentEnabled<BuildingBarracksUnitEnqueue>(entity, false);
            }
        }
    }

    public struct BuildingBarracksUnitEnqueue : IComponentData, IEnableableComponent
    {
        public UnitType UnitType;
    }

    public struct BuildingBarracks : IComponentData
    {
        public float Progress;
        public float ProgressMax;
        public UnitType ActiveUnitType;
        public float3 RallyPositionOffset;
        public bool OnUnitQueueChanged;
    }

    [InternalBufferCapacity(10)]
    public struct SpawnUnitTypeBuffer : IBufferElementData
    {
        public UnitType UnitType;
    }
}