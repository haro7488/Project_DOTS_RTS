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

                var spawnUnitTypeDynamicBuffer = AddBuffer<SpawnUnitTypeBuffer>(entity);
                spawnUnitTypeDynamicBuffer.Add(new SpawnUnitTypeBuffer
                {
                    UnitType = UnitType.Soldier,
                });
                spawnUnitTypeDynamicBuffer.Add(new SpawnUnitTypeBuffer
                {
                    UnitType = UnitType.Soldier,
                });
                spawnUnitTypeDynamicBuffer.Add(new SpawnUnitTypeBuffer
                {
                    UnitType = UnitType.Scout
                });
            }
        }
    }

    public struct BuildingBarracks : IComponentData
    {
        public float Progress;
        public float ProgressMax;
        public UnitType ActiveUnitType;
        public float3 RallyPositionOffset;
    }

    [InternalBufferCapacity(10)]
    public struct SpawnUnitTypeBuffer : IBufferElementData
    {
        public UnitType UnitType;
    }
}