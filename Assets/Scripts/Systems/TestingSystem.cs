using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace DotsRts.Systems
{
    public partial struct TestingSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // var unitCount = 0;
            // foreach (var friendly in
            //          SystemAPI.Query<
            //              RefRW<Friendly>>())
            // {
            //     unitCount++;
            // }
            //
            // Debug.Log("Unit count: " + unitCount);
        }
    }
}