using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;
using BoxCollider = UnityEngine.BoxCollider;

namespace DotsRts.MonoBehaviours
{
    public class BuildingPlacementManager : MonoBehaviour
    {
        [SerializeField] private BuildingTypeSO buildingTypeSo;

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (CanPlaceBuilding())
                {
                    var mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
                    var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

                    var entityQuery = entityManager.CreateEntityQuery(typeof(EntitiesReferences));
                    var entitiesReferences = entityQuery.GetSingleton<EntitiesReferences>();

                    var spawnedEntity = entityManager.Instantiate(entitiesReferences.BuildingTowerPrefabEntity);
                    entityManager.SetComponentData(spawnedEntity, LocalTransform.FromPosition(mouseWorldPosition));
                }
            }
        }

        private bool CanPlaceBuilding()
        {
            var mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var entityQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
            var physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();
            var collisionWorld = physicsWorldSingleton.CollisionWorld;
            var collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.BUILDINGS_LAYER,
                GroupIndex = 0,
            };

            var boxCollider = buildingTypeSo.Prefab.GetComponent<BoxCollider>();
            var bonusExtents = 1.1f;
            var distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);
            if (collisionWorld.OverlapBox(mouseWorldPosition, quaternion.identity,
                    boxCollider.size * 0.5f * bonusExtents, ref distanceHitList, collisionFilter))
            {
                return false;
            }

            distanceHitList.Clear();
            if (collisionWorld.OverlapSphere(mouseWorldPosition,
                    buildingTypeSo.BuildingDistanceMin,
                    ref distanceHitList, collisionFilter))
            {
                foreach (var distanceHit in distanceHitList)
                {
                    if (entityManager.HasComponent<BuildingTypeSOHolder>(distanceHit.Entity))
                    {
                        var buildingTypeSoHolder =
                            entityManager.GetComponentData<BuildingTypeSOHolder>(distanceHit.Entity);
                        if (buildingTypeSoHolder.BuildingType == buildingTypeSo.BuildingType)
                        {
                            // Same type too close
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}