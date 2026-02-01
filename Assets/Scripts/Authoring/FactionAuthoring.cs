using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class FactionAuthoring : MonoBehaviour
    {
        public FactionType FactionType;

        private class FactionAuthoringBaker : Baker<FactionAuthoring>
        {
            public override void Bake(FactionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Faction
                {
                    FactionType = authoring.FactionType,
                });
            }
        }
    }

    public struct Faction : IComponentData
    {
        public FactionType FactionType;
    }
}