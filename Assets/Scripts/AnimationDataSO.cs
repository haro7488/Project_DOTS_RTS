using UnityEngine;

namespace DotsRts
{
    public enum AnimationType
    {
        None,
        SoldierIdle,
        SoldierWalk,
        ZombieIdle,
        ZombieWalk,
        SoldierAim,
        SoldierShoot,
        ZombieAttack,
        ScoutIdle,
        ScoutWalk,
        ScoutShoot,
        ScoutAim,
    }

    [CreateAssetMenu]
    public class AnimationDataSO : ScriptableObject
    {
        public AnimationType AnimationType;
        public Mesh[] MeshArray;
        public float FrameTimerMax;
        
        public static bool IsAnimationUninterruptible(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.SoldierShoot:
                case AnimationType.ScoutShoot:
                case AnimationType.ZombieAttack:
                    return true;
                default:
                    return false;
            }
        }
    }
}