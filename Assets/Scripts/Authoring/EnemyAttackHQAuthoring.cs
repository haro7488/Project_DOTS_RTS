using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class EnemyAttackHQAuthoring : MonoBehaviour
    {
        private class EnemyAttackHQAuthoringBaker : Baker<EnemyAttackHQAuthoring>
        {
            public override void Bake(EnemyAttackHQAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<EnemyAttackHQ>(entity);
            }
        }
    }

    public struct EnemyAttackHQ : IComponentData
    {
    }
}