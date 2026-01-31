using UnityEngine;

namespace DotsRts
{
    [CreateAssetMenu]
    public class AnimationDataSO : ScriptableObject
    {
        public Mesh[] MeshArray;
        public float FrameTimerMax;
    }
}