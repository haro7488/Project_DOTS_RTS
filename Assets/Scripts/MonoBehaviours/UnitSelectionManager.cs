using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace DotsRts.MonoBehaviours
{
    public class UnitSelectionManager : MonoBehaviour
    {
        public static UnitSelectionManager Instance { get; private set; }

        public event EventHandler OnSelectionAreaStart;
        public event EventHandler OnSelectionAreaEnd;

        private Vector2 _selectionStartMousePosition;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _selectionStartMousePosition = Input.mousePosition;
                Debug.Log("Selection started at: " + _selectionStartMousePosition);
                OnSelectionAreaStart?.Invoke(this, EventArgs.Empty);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 selectionEndMousePosition = Input.mousePosition;

                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().Build(entityManager);

                var entityArray = entityQuery.ToEntityArray(Allocator.Temp);
                var selectedArray = entityQuery.ToComponentDataArray<Selected>(Allocator.Temp);
                for (var i = 0; i < entityArray.Length; i++)
                {
                    var entity = entityArray[i];
                    entityManager.SetComponentEnabled<Selected>(entity, false);
                    var selected = selectedArray[i];
                    selected.OnDeselected = true;
                    entityManager.SetComponentData(entityArray[i], selected);
                }

                var selectionAreaRect = GetSelectionAreaRect();
                var selectionAreaSize = selectionAreaRect.width + selectionAreaRect.height;
                var multipleSelectionSizeMin = 40f;
                var isMultipleSelection = selectionAreaSize > multipleSelectionSizeMin;
                if (isMultipleSelection)
                {
                    entityQuery = new EntityQueryBuilder(Allocator.Temp)
                        .WithAll<LocalTransform, Unit>().WithPresent<Selected>().Build(entityManager);
                    entityArray = entityQuery.ToEntityArray(Allocator.Temp);
                    var localTransformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
                    for (int i = 0; i < localTransformArray.Length; i++)
                    {
                        var unitLocalTransform = localTransformArray[i];
                        var unitScreenPosition = Camera.main.WorldToScreenPoint(unitLocalTransform.Position);
                        if (selectionAreaRect.Contains(unitScreenPosition))
                        {
                            entityManager.SetComponentEnabled<Selected>(entityArray[i], true);
                            var selected = entityManager.GetComponentData<Selected>(entityArray[i]);
                            selected.OnSelected = true;
                            entityManager.SetComponentData(entityArray[i], selected);
                        }
                    }
                }
                else
                {
                    entityQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                    var physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();
                    var collisionWorld = physicsWorldSingleton.CollisionWorld;
                    UnityEngine.Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var raycastInput = new RaycastInput
                    {
                        Start = cameraRay.GetPoint(0f),
                        End = cameraRay.GetPoint(9999f),
                        Filter = new CollisionFilter
                        {
                            BelongsTo = ~0u,
                            CollidesWith = 1u << GameAssets.UNITS_LAYER,
                            GroupIndex = 0,
                        }
                    };
                    if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit))
                    {
                        if (entityManager.HasComponent<Unit>(raycastHit.Entity) && entityManager.HasComponent<Selected>(raycastHit.Entity))
                        {
                            entityManager.SetComponentEnabled<Selected>(raycastHit.Entity, true);
                            var selected = entityManager.GetComponentData<Selected>(raycastHit.Entity);
                            selected.OnSelected = true;
                            entityManager.SetComponentData(raycastHit.Entity, selected);
                        }
                    }
                }

                OnSelectionAreaEnd?.Invoke(this, EventArgs.Empty);
            }

            if (Input.GetMouseButtonDown(1))
            {
                var mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();

                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entityQuery = new EntityQueryBuilder(Allocator.Temp)
                    .WithAll<UnitMover, Selected>().Build(entityManager);

                var unitMoverArray = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
                var movePositionArray = GenerateMovePositionArray(mouseWorldPosition, unitMoverArray.Length);
                for (int i = 0; i < unitMoverArray.Length; i++)
                {
                    var unitMover = unitMoverArray[i];
                    unitMover.TargetPosition = movePositionArray[i];
                    unitMoverArray[i] = unitMover;
                }

                entityQuery.CopyFromComponentDataArray(unitMoverArray);
            }
        }

        public Rect GetSelectionAreaRect()
        {
            var selectionEndMousePosition = Input.mousePosition;
            var lowerLeftConer = new Vector2(
                Mathf.Min(_selectionStartMousePosition.x, selectionEndMousePosition.x),
                Mathf.Min(_selectionStartMousePosition.y, selectionEndMousePosition.y));

            var upperRightCorner = new Vector2(
                Mathf.Max(_selectionStartMousePosition.x, selectionEndMousePosition.x),
                Mathf.Max(_selectionStartMousePosition.y, selectionEndMousePosition.y));
            return new Rect(
                lowerLeftConer.x,
                lowerLeftConer.y,
                upperRightCorner.x - lowerLeftConer.x,
                upperRightCorner.y - lowerLeftConer.y
            );
        }

        private NativeArray<float3> GenerateMovePositionArray(float3 targetPosition, int positionCount)
        {
            var positionArray = new NativeArray<float3>(positionCount, Allocator.Temp);
            if (positionCount == 0)
            {
                return positionArray;
            }

            positionArray[0] = targetPosition;
            if (positionCount == 1)
            {
                return positionArray;
            }

            var ringSize = 2.2f;
            var ring = 0;
            int positionIndex = 1;

            while (positionIndex < positionCount)
            {
                var ringPositionCount = 3 + ring * 2;
                for (int i = 0; i < ringPositionCount; i++)
                {
                    var angle = i * (math.PI2 / ringPositionCount);
                    var ringVector = math.rotate(quaternion.RotateY(angle), new float3(ringSize * (ring + 1), 0, 0));
                    var ringPosition = targetPosition + ringVector;

                    positionArray[positionIndex] = ringPosition;
                    positionIndex++;

                    if (positionIndex >= positionCount)
                    {
                        break;
                    }
                }

                ring++;
            }

            return positionArray;
        }
    }
}