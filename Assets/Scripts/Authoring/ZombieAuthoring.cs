using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class ZombieAuthoring : MonoBehaviour
    {
        private class ZombieAuthoringBaker : Baker<ZombieAuthoring>
        {
            public override void Bake(ZombieAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Zombie>(entity);
            }
        }
    }

    public struct Zombie : IComponentData
    {
    }
}