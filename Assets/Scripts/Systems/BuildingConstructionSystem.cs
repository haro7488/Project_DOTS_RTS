using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DotsRts.Systems
{
    public partial struct BuildingConstructionSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (localTransform,
                         buildingConstruction,
                         entity)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<BuildingConstruction>>().WithEntityAccess())
            {
                var visualLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(buildingConstruction.ValueRO.VisualEntity);

                visualLocalTransform.ValueRW.Position = math.lerp(buildingConstruction.ValueRO.StartPosition,
                    buildingConstruction.ValueRO.EndPosition,
                    buildingConstruction.ValueRO.ConstructionTimer / buildingConstruction.ValueRO.ConstructionTimerMax);
                
                
                buildingConstruction.ValueRW.ConstructionTimer += SystemAPI.Time.DeltaTime;
                if (buildingConstruction.ValueRO.ConstructionTimer >= buildingConstruction.ValueRO.ConstructionTimerMax)
                {
                    var spawnedBuildingEntity =
                        entityCommandBuffer.Instantiate(buildingConstruction.ValueRO.FinalPrefabEntity);
                    entityCommandBuffer.SetComponent(spawnedBuildingEntity,
                        LocalTransform.FromPosition(localTransform.ValueRO.Position));

                    entityCommandBuffer.DestroyEntity(buildingConstruction.ValueRO.VisualEntity);
                    entityCommandBuffer.DestroyEntity(entity);
                }
            }
        }
    }
}