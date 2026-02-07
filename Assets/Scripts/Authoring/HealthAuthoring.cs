using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class HealthAuthoring : MonoBehaviour
    {
        public int HealthAmount = 1;
        public int HealthAmountMax = 1;

        private class HealthAuthoringBaker : Baker<HealthAuthoring>
        {
            public override void Bake(HealthAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Health
                {
                    HealthAmount = authoring.HealthAmount,
                    HealthAmountMax = authoring.HealthAmountMax,
                    OnHealthChanged = true,
                });
            }
        }
    }

    public struct Health : IComponentData
    {
        public int HealthAmount;
        public int HealthAmountMax;
        public bool OnHealthChanged;
        public bool OnDead;
    }
}