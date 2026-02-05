using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class DOTSEventsManager : MonoBehaviour
    {
        public static DOTSEventsManager Instance { get; private set; }

        public event EventHandler OnBarracksUnitQueueChanged;

        private void Awake()
        {
            Instance = this;
        }

        public void TriggerOnBarracksUnitQueueChanged(NativeList<Entity> entityNativeList)
        {
            foreach (var entity in entityNativeList)
            {
                OnBarracksUnitQueueChanged?.Invoke(entity, EventArgs.Empty);
            }
        }
    }
}