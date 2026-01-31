using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DotsRts.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct ShootLightSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EntitiesReferences>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var shootAttack in SystemAPI.Query<RefRO<ShootAttack>>())
            {
                var entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

                var onShoot = shootAttack.ValueRO.OnShoot;
                if (onShoot.IsTriggered)
                {
                    var shootLightEntity = state.EntityManager.Instantiate(entitiesReferences.ShootLightPrefabEntity);
                    SystemAPI.SetComponent(shootLightEntity, LocalTransform.FromPosition(onShoot.ShootFromPosition));
                }
            }
        }
    }
}