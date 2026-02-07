using System;
using DotsRts.Systems;
using Unity.Entities;
using UnityEngine;

namespace DotsRts.MonoBehaviours
{
    public class GridSystemDebug : MonoBehaviour
    {
        public static GridSystemDebug Instance { get; private set; }
        [SerializeField] private Transform _debugPrefab;

        private bool _isInit;
        private GridSystemDebugSingle[,] _gridSystemDebugSingleArray;

        private void Awake()
        {
            Instance = this;
        }

        public void InitializeGrid(GridSystem.GridSystemData gridSystemData)
        {
            if (_isInit)
            {
                return;
            }

            _isInit = true;

            _gridSystemDebugSingleArray = new GridSystemDebugSingle[gridSystemData.Width, gridSystemData.Height];
            for (int x = 0; x < gridSystemData.Width; x++)
            {
                for (int y = 0; y < gridSystemData.Height; y++)
                {
                    var debugTransform = Instantiate(_debugPrefab);
                    var gridSystemDebugSingle = debugTransform.GetComponent<GridSystemDebugSingle>();
                    gridSystemDebugSingle.Setup(x, y, gridSystemData.GridNodeSize);

                    _gridSystemDebugSingleArray[x, y] = gridSystemDebugSingle;
                }
            }
        }

        public void UpdateGrid(GridSystem.GridSystemData gridSystemData)
        {
            for (int x = 0; x < gridSystemData.Width; x++)
            {
                for (int y = 0; y < gridSystemData.Height; y++)
                {
                    var gridSystemDebugSingle = _gridSystemDebugSingleArray[x, y];

                    var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                    var index = GridSystem.CalculateIndex(x, y, gridSystemData.Width);
                    var gridNodeEntity = gridSystemData.GridMap.GridEntityArray[index];
                    var gridNode = entityManager.GetComponentData<GridSystem.GridNode>(gridNodeEntity);
                    gridSystemDebugSingle.SetColor(gridNode.Data == 0 ? Color.white : Color.blue);
                }
            }
        }
    }
}