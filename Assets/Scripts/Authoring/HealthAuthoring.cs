using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class HealthAuthoring : MonoBehaviour
    {
        public int HealthAmount = 1;
        
        private class HealthAuthoringBaker : Baker<HealthAuthoring>
        {
            public override void Bake(HealthAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Health
                {
                    HealthAmount = authoring.HealthAmount,
                });
            }
        }
    }

    public struct Health : IComponentData
    {
        public int HealthAmount;
    }
}