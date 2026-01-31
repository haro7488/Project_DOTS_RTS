using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class LoseTargetAuthoring : MonoBehaviour
    {
        public float LoseTargetDistance;

        private class LoseTargetAuthoringBaker : Baker<LoseTargetAuthoring>
        {
            public override void Bake(LoseTargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new LoseTarget
                {
                    LoseTargetDistance = authoring.LoseTargetDistance,
                });
            }
        }
    }

    public struct LoseTarget : IComponentData
    {
        public float LoseTargetDistance;
    }
}