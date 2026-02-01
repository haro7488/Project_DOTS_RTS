using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class UnitTypeHolderAuthoring : MonoBehaviour
    {
        public UnitType UnitType;

        private class UnitTypeHolderAuthoringBaker : Baker<UnitTypeHolderAuthoring>
        {
            public override void Bake(UnitTypeHolderAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new UnitTypeHolder
                {
                    UnitType = authoring.UnitType,
                });
            }
        }
    }

    public struct UnitTypeHolder : IComponentData
    {
        public UnitType UnitType;
    }
}