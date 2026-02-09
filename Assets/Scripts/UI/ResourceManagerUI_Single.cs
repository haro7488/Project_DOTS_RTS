using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DotsRts.UI
{
    public class ResourceManagerUI_Single : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        public void Setup(ResourceTypeSO resourceTypeSo)
        {
            _image.sprite = resourceTypeSo.Sprite;
            _textMeshProUGUI.text = "0";
        }

        public void UpdateAmount(int amount)
        {
            _textMeshProUGUI.text = amount.ToString();
        }
    }
}