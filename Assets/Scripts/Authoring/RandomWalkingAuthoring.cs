using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace DotsRts
{
    public class RandomWalkingAuthoring : MonoBehaviour
    {
        public float3 TargetPosition = float3.zero;
        public float3 OriginPosition = float3.zero;
        public float DistanceMin = 2f;
        public float DistanceMax = 5f;
        public uint RandomSeed = 1;
        
        private class RandomWalkingAuthoringBaker : Baker<RandomWalkingAuthoring>
        {
            public override void Bake(RandomWalkingAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new RandomWalking
                {
                    TargetPosition = authoring.TargetPosition,
                    OriginPosition = authoring.OriginPosition,
                    DistanceMin = authoring.DistanceMin,
                    DistanceMax = authoring.DistanceMax,
                    Random = new Random(authoring.RandomSeed),
                });
            }
        }
    }
    
    public struct RandomWalking : IComponentData
    {
        public float3 TargetPosition;
        public float3 OriginPosition;
        public float DistanceMin;
        public float DistanceMax;
        public Random Random;
    }
}