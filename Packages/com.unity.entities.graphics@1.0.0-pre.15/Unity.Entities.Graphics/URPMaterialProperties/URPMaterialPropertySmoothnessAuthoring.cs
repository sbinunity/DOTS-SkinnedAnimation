#if URP_10_0_0_OR_NEWER
using Unity.Entities;

namespace Unity.Rendering
{
    [MaterialProperty("_Smoothness")]
    public struct URPMaterialPropertySmoothness : IComponentData
    {
        public float Value;
    }

    [UnityEngine.DisallowMultipleComponent]
    public class URPMaterialPropertySmoothnessAuthoring : UnityEngine.MonoBehaviour
    {
        [Unity.Entities.RegisterBinding(typeof(URPMaterialPropertySmoothness), "Value")]
        public float Value;

        class URPMaterialPropertySmoothnessBaker : Unity.Entities.Baker<URPMaterialPropertySmoothnessAuthoring>
        {
            public override void Bake(URPMaterialPropertySmoothnessAuthoring authoring)
            {
                Unity.Rendering.URPMaterialPropertySmoothness component = default(Unity.Rendering.URPMaterialPropertySmoothness);
                component.Value = authoring.Value;
                AddComponent(component);
            }
        }
    }
}
#endif
