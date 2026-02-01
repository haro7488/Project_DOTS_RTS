using UnityEngine;

namespace DotsRts
{
    public enum BuildingType
    {
        None,
        ZombieSpawner,
        Tower,
        Barracks,
    }

    [CreateAssetMenu]
    public class BuildingTypeSO : ScriptableObject
    {
        public BuildingType BuildingType;
    }
}