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

            foreach (var (buildingBarracks,
                         spawnUnitTypeDynamicBuffer,
                         buildingBarracksUnitEnqueue,
                         buildingBarracksUnitEnqueueEnabled)
                     in SystemAPI.Query<
                         RefRW<BuildingBarracks>,
                         DynamicBuffer<SpawnUnitTypeBuffer>,
                         RefRO<BuildingBarracksUnitEnqueue>,
                         EnabledRefRW<BuildingBarracksUnitEnqueue>>())
            {
                spawnUnitTypeDynamicBuffer.Add(new SpawnUnitTypeBuffer
                {
                    UnitType = buildingBarracksUnitEnqueue.ValueRO.UnitType,
                });
                buildingBarracksUnitEnqueueEnabled.ValueRW = false;
                
                buildingBarracks.ValueRW.OnUnitQueueChanged = true;
            }

            foreach (var (localTransform,
                         buildingBarracks,
                         spawnUnitTypeDynamicBuffer)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<BuildingBarracks>,
                         DynamicBuffer<SpawnUnitTypeBuffer>>())
            {
                if (spawnUnitTypeDynamicBuffer.IsEmpty)
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

                buildingBarracks.ValueRW.Progress += SystemAPI.Time.DeltaTime;

                if (buildingBarracks.ValueRO.Progress < buildingBarracks.ValueRO.ProgressMax)
                {
                    continue;
                }

                buildingBarracks.ValueRW.Progress = 0f;

                var unitType = spawnUnitTypeDynamicBuffer[0].UnitType;
                var unitTypeSo = GameAssets.Instance.UnitTypeListSO.GetUnitTypeSO(unitType);

                spawnUnitTypeDynamicBuffer.RemoveAt(0);
                buildingBarracks.ValueRW.OnUnitQueueChanged = true;

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