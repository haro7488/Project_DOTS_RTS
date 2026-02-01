using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DotsRts
{
    public class ZombieSpawnerAuthoring : MonoBehaviour
    {
        public float TimerMax = 2f;
        public float RandomWalkingDistanceMin = 5f;
        public float RandomWalkingDistanceMax = 10f;
        public int NearbyZombieAmountMax = 5;
        public float NearbyZombieAmountDistance = 10f;

        private class ZombieSpawnerAuthoringBaker : Baker<ZombieSpawnerAuthoring>
        {
            public override void Bake(ZombieSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ZombieSpawner
                {
                    TimerMax = authoring.TimerMax,
                    RandomWalkingDistanceMin = authoring.RandomWalkingDistanceMin,
                    RandomWalkingDistanceMax = authoring.RandomWalkingDistanceMax,
                    NearbyZombieAmountMax = authoring.NearbyZombieAmountMax,
                    NearbyZombieAmountDistance = authoring.NearbyZombieAmountDistance,
                });
            }
        }
    }

    public struct ZombieSpawner : IComponentData
    {
        public float Timer;
        public float TimerMax;
        public float RandomWalkingDistanceMin;
        public float RandomWalkingDistanceMax;
        public int NearbyZombieAmountMax;
        public float NearbyZombieAmountDistance;
    }
}