using DotsRts.MonoBehaviours;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DotsRts.Systems
{
    public partial struct BuildingBarracksSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EntitiesReferences>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

            foreach (var (localTransform,
                         buildingBarracks,
                         spawnUnitTypeDynamicBuffer)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<BuildingBarracks>,
                         DynamicBuffer<SpawnUnitTypeBuffer>>())
            {
                buildingBarracks.ValueRW.Progress += SystemAPI.Time.DeltaTime;

                if (buildingBarracks.ValueRO.Progress < buildingBarracks.ValueRO.ProgressMax)
                {
                    continue;
                }

                if (buildingBarracks.ValueRO.ActiveUnitType != spawnUnitTypeDynamicBuffer[0].UnitType)
                {
                    buildingBarracks.ValueRW.ActiveUnitType = spawnUnitTypeDynamicBuffer[0].UnitType;
                    var activeUnitTypeSo =
                        GameAssets.Instance.UnitTypeListSO.GetUnitTypeSO(buildingBarracks.ValueRO.ActiveUnitType);
                    buildingBarracks.ValueRW.ProgressMax = activeUnitTypeSo.ProgressMax;
                }

                buildingBarracks.ValueRW.Progress = 0f;

                var unitType = spawnUnitTypeDynamicBuffer[0].UnitType;
                var unitTypeSo = GameAssets.Instance.UnitTypeListSO.GetUnitTypeSO(unitType);

                spawnUnitTypeDynamicBuffer.RemoveAt(0);
                var spawnedUnitEntity = state.EntityManager.Instantiate(unitTypeSo.GetPrefabEntity(entitiesReferences));
                SystemAPI.SetComponent(spawnedUnitEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));

                SystemAPI.SetComponent(spawnedUnitEntity, new MoveOverride
                {
                    TargetPosition = localTransform.ValueRO.Position + buildingBarracks.ValueRO.RallyPositionOffset
                });
                SystemAPI.SetComponentEnabled<MoveOverride>(spawnedUnitEntity, true);
            }
        }
    }
}