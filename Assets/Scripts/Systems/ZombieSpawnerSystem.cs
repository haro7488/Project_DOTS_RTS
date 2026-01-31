using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DotsRts.Systems
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct ZombieSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EntitiesReferences>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
            var entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (localTransform,
                         zombieSpawner)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<ZombieSpawner>>())
            {
                zombieSpawner.ValueRW.Timer -= SystemAPI.Time.DeltaTime;
                if (zombieSpawner.ValueRO.Timer > 0f)
                {
                    continue;
                }

                zombieSpawner.ValueRW.Timer = zombieSpawner.ValueRO.TimerMax;

                var zombieEntity = state.EntityManager.Instantiate(entitiesReferences.ZombiePrefabEntity);
                state.EntityManager.SetComponentData(zombieEntity, 
                    LocalTransform.FromPosition(localTransform.ValueRO.Position));
                
                entityCommandBuffer.AddComponent(zombieEntity, new RandomWalking
                {
                    OriginPosition = localTransform.ValueRO.Position,
                    TargetPosition = localTransform.ValueRO.Position,
                    DistanceMin = zombieSpawner.ValueRO.RandomWalkingDistanceMin,
                    DistanceMax = zombieSpawner.ValueRO.RandomWalkingDistanceMax,
                    Random = new Random((uint)zombieEntity.Index),
                });
            }
        }
    }
}