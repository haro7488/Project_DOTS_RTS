using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class SetupUnitMoverDefaultPositionAuthoring : MonoBehaviour
    {
        private class
            SetupUnitMoverDefaultPositionAuthoringBaker : Baker<SetupUnitMoverDefaultPositionAuthoring>
        {
            public override void Bake(SetupUnitMoverDefaultPositionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<SetupUnitMoverDefaultPosition>(entity);
            }
        }
    }
    
    public struct SetupUnitMoverDefaultPosition : IComponentData
    {
    }
}