using UnityEngine;

namespace DotsRts
{
    public enum ResourceType
    {
        None,
        Iron,
        Gold,
        Oil,
    }
    
    [CreateAssetMenu]
    public class ResourceTypeSO : ScriptableObject
    {
        public ResourceType ResourceType;
        public Sprite Sprite;
    }
}