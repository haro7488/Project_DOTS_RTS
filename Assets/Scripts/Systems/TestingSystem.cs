using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace DotsRts.Systems
{
    public partial struct TestingSystem : ISystem
    {
        // [BurstCompile]
        // public void OnUpdate(ref SystemState state)
        // {
        //     var unitCount = 0;
        //     foreach (var (localTransform,
        //                  unitMover,
        //                  physicsVelocity) in
        //              SystemAPI.Query<
        //                  RefRW<LocalTransform>,
        //                  RefRO<UnitMover>,
        //                  RefRW<PhysicsVelocity>>().WithPresent<Selected>())
        //     {
        //         unitCount++;
        //     }
        //
        //     Debug.Log("Unit count: " + unitCount);
        // }
    }
}