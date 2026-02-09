using System;
using System.Collections.Generic;
using DotsRts.MonoBehaviours;
using UnityEngine;

namespace DotsRts.UI
{
    public class ResourceManagerUI : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _template;
        [SerializeField] private ResourceTypeListSO _resourceTypeListSo;

        private Dictionary<ResourceType, ResourceManagerUI_Single> _resourceTypeUiSingleDict;

        private void Awake()
        {
            _resourceTypeUiSingleDict = new();
            _template.gameObject.SetActive(false);
        }

        private void Start()
        {
            ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;

            Setup();
            UpdateAmounts();
        }

        private void ResourceManager_OnResourceAmountChanged(object sender, EventArgs e)
        {
            UpdateAmounts();
        }

        private void Setup()
        {
            foreach (Transform child in _container)
            {
                if (child == _template)
                {
                    continue;
                }

                Destroy(child.gameObject);
            }

            foreach (var resourceTypeSo in _resourceTypeListSo.ResourceTypeSOList)
            {
                var resourceTransform = Instantiate(_template, _container);
                resourceTransform.gameObject.SetActive(true);
                var resourceManagerUISingle = resourceTransform.GetComponent<ResourceManagerUI_Single>();
                resourceManagerUISingle.Setup(resourceTypeSo);

                _resourceTypeUiSingleDict[resourceTypeSo.ResourceType] = resourceManagerUISingle;
            }
        }

        private void UpdateAmounts()
        {
            foreach (var resourceTypeSo in _resourceTypeListSo.ResourceTypeSOList)
            {
                _resourceTypeUiSingleDict[resourceTypeSo.ResourceType].UpdateAmount(
                    ResourceManager.Instance.GetResourceAmount(resourceTypeSo.ResourceType));
            }
        }
    }
}