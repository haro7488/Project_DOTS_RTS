using System;
using DotsRts.MonoBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace DotsRts.UI
{
    public class BuildingPlacementManagerUI_ButtonSingle : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _selectedImage;

        private BuildingTypeSO _buildingTypeSo;

        public void Setup(BuildingTypeSO buildingTypeSo)
        {
            _buildingTypeSo = buildingTypeSo;

            GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingPlacementManager.Instance.BuildingTypeSo = buildingTypeSo;
            });
            _iconImage.sprite = buildingTypeSo.Sprite;
        }

        public void ShowSelected()
        {
            _selectedImage.enabled = true;
        }

        public void HideSelected()
        {
            _selectedImage.enabled = false;
        }
    }
}