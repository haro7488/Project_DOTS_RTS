using System;
using System.Collections.Generic;
using DotsRts.MonoBehaviours;
using UnityEngine;

namespace DotsRts.UI
{
    public class BuildingPlacementManagerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _buildingContainer;
        [SerializeField] private RectTransform _buildingTemplate;
        [SerializeField] private BuildingTypeListSO _buildingTypeListSo;

        private Dictionary<BuildingTypeSO, BuildingPlacementManagerUI_ButtonSingle> _buildingButtonDict = new();

        private void Awake()
        {
            foreach (var buildingTypeSo in _buildingTypeListSo.BuildingTypeSOList)
            {
                if (!buildingTypeSo.ShowInBuildingPlacementManagerUI)
                {
                    continue;
                }

                var buildingRectTransform = Instantiate(_buildingTemplate, _buildingContainer);
                buildingRectTransform.gameObject.SetActive(true);

                var buttonSingle = buildingRectTransform.GetComponent<BuildingPlacementManagerUI_ButtonSingle>();

                _buildingButtonDict[buildingTypeSo] = buttonSingle;
                buttonSingle.Setup(buildingTypeSo);
            }
        }

        private void Start()
        {
            BuildingPlacementManager.Instance.OnActiveBuildingTypeSOChanged +=
                BuildingPlacementManager_OnActiveBuildingTypeSOChanged;
            UpdateSelectedVisual();
        }

        private void BuildingPlacementManager_OnActiveBuildingTypeSOChanged(object sender, EventArgs e)
        {
            UpdateSelectedVisual();
        }

        private void UpdateSelectedVisual()
        {
            foreach (var buttonSingle in _buildingButtonDict.Values)
            {
                buttonSingle.HideSelected();
            }

            _buildingButtonDict[BuildingPlacementManager.Instance.BuildingTypeSo].ShowSelected();
        }
    }
}