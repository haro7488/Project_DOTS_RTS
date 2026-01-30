using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DotsRts.MonoBehaviours
{
    public class UnitSelectionManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();

                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover>().Build(entityManager);
                var unitMoverArray = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
                for (int i = 0; i < unitMoverArray.Length; i++)
                {
                    var unitMover = unitMoverArray[i];
                    unitMover.TargetPosition = mouseWorldPosition;
                    unitMoverArray[i] = unitMover;
                    // entityManager.SetComponentData(entityArray[i], unitMover);
                }

                entityQuery.CopyFromComponentDataArray(unitMoverArray);
            }
        }
    }
}