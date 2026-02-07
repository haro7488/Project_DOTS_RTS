using System;
using UnityEngine;

namespace DotsRts.UI
{
    public class ResetPositionUI: MonoBehaviour
    {
        private void Awake()
        {
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            Destroy(this);
        }
    }
}