using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class FriendlyAuthoring : MonoBehaviour
    {
        private class FriendlyAuthoringBaker : Baker<FriendlyAuthoring>
        {
            public override void Bake(FriendlyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Friendly>(entity);
            }
        }
    }

    public struct Friendly : IComponentData
    {
    }
}