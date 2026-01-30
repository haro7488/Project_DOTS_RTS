using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class UnitAuthoring : MonoBehaviour
    {
        public Faction faction;

        private class UnitAuthoringBaker : Baker<UnitAuthoring>
        {
            public override void Bake(UnitAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Unit
                {
                    Faction = authoring.faction,
                });
            }
        }
    }

    public struct Unit : IComponentData
    {
        public Faction Faction;
    }
}