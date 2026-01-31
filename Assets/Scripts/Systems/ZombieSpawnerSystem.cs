using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DotsRts.Systems
{
    public partial struct ZombieSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EntitiesReferences>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

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
            }
        }
    }
}