using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveSpeedAuthoring : MonoBehaviour
    {
        private class MoveSpeedAuthoringBaker : Baker<MoveSpeedAuthoring>
        {
            public override void Bake(MoveSpeedAuthoring authoring)
            {
            }
        }
    }
}