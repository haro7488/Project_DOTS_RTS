using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotsRts.MonoBehaviours
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }

        public event EventHandler OnResourceAmountChanged;

        [SerializeField] private ResourceTypeListSO _resourceTypeListSO;

        private Dictionary<ResourceType, int> _resourceTypeAmountDict;

        private void Awake()
        {
            Instance = this;

            _resourceTypeAmountDict = new();
            foreach (var resourceTypeSo in _resourceTypeListSO.ResourceTypeSOList)
            {
                _resourceTypeAmountDict[resourceTypeSo.ResourceType] = 0;
            }
        }

        public void AddResourceAmount(ResourceType resourceType, int amount)
        {
            _resourceTypeAmountDict[resourceType] += amount;
            OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
        }

        public int GetResourceAmount(ResourceType resourceType)
        {
            return _resourceTypeAmountDict[resourceType];
        }
    }
}