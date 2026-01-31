using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DotsRts
{
    public class UnitMoverAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
        public float RotationSpeed;

        private class MoveSpeedAuthoringBaker : Baker<UnitMoverAuthoring>
        {
            public override void Bake(UnitMoverAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new UnitMover
                {
                    MoveSpeed = authoring.MoveSpeed,
                    RotationSpeed = authoring.RotationSpeed,
                    TargetPosition = authoring.transform.position,
                });
            }
        }
    }
    
    public struct UnitMover : IComponentData
    {
        public float MoveSpeed;
        public float RotationSpeed;
        public float3 TargetPosition;
    }
}