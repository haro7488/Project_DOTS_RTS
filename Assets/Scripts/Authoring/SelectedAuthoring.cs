using Unity.Entities;
using UnityEngine;

namespace DotsRts
{
    public class SelectedAuthoring : MonoBehaviour
    {
        public GameObject VisualGameObject;
        public float ShowScale = 1f;

        private class SelectedAuthoringBaker : Baker<SelectedAuthoring>
        {
            public override void Bake(SelectedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Selected
                {
                    VisualEntity = GetEntity(authoring.VisualGameObject, TransformUsageFlags.Dynamic),
                    ShowScale = authoring.ShowScale,
                });
                SetComponentEnabled<Selected>(entity, false);
            }
        }
    }

    public struct Selected : IComponentData, IEnableableComponent
    {
        public Entity VisualEntity;
        public float ShowScale;

        public bool OnSelected;
        public bool OnDeselected;
    }
}