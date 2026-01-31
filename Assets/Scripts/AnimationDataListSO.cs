using System.Collections.Generic;
using UnityEngine;

namespace DotsRts
{
    [CreateAssetMenu]
    public class AnimationDataListSO : ScriptableObject
    {
        public List<AnimationDataSO> AnimationDataSOList;

        public AnimationDataSO GetAnimationDataSO(AnimationType animationType)
        {
            foreach (var animationDataSo in AnimationDataSOList)
            {
                if (animationDataSo.AnimationType == animationType)
                {
                    return animationDataSo;
                }
            }

            Debug.Log("AnimationDataSO not found for AnimationType: " + animationType);
            return null;
        }
    }
}