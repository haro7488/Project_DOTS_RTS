using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class ZombieSpawnerAuthoring : MonoBehaviour
    {
        public float TimerMax = 2f;

        private class ZombieSpawnerAuthoringBaker : Baker<ZombieSpawnerAuthoring>
        {
            public override void Bake(ZombieSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ZombieSpawner
                {
                    TimerMax = authoring.TimerMax,
                });
            }
        }
    }

    public struct ZombieSpawner : IComponentData
    {
        public float Timer;
        public float TimerMax;
    }
}