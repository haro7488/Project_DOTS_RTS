using UnityEngine;

namespace DotsRts
{
    public enum AnimationType
    {
        None,
        SoldierIdle,
        SoldierWalk,
    }

    [CreateAssetMenu]
    public class AnimationDataSO : ScriptableObject
    {
        public AnimationType AnimationType;
        public Mesh[] MeshArray;
        public float FrameTimerMax;
    }
}