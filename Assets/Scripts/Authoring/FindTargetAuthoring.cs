using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class FindTargetAuthoring : MonoBehaviour
    {
        public float Range;
        public Faction TargetFaction;
        public float TimerMax;
        
        private class FindTargetAuthoringBaker : Baker<FindTargetAuthoring>
        {
            public override void Bake(FindTargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new FindTarget
                {
                    Range = authoring.Range,
                    TargetFaction = authoring.TargetFaction,
                    TimerMax = authoring.TimerMax,
                });
            }
        }
    }
    
    public struct FindTarget : IComponentData
    {
        public float Range;
        public Faction TargetFaction;
        public float Timer;
        public float TimerMax;
    }
}