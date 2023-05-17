#if HDRP_10_0_0_OR_NEWER
using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_ThicknessRemap"       )]
    public struct HDRPMaterialPropertyThicknessRemap : IComponentData { public float4 Value; }

    [UnityEngine.DisallowMultipleComponent]
    public class HDRPMaterialPropertyThicknessRemapAuthoring : UnityEngine.MonoBehaviour
    {
        [RegisterBinding(typeof(HDRPMaterialPropertyThicknessRemap), "Value.x", true)]
        [RegisterBinding(typeof(HDRPMaterialPropertyThicknessRemap), "Value.y", true)]
        [RegisterBinding(typeof(HDRPMaterialPropertyThicknessRemap), "Value.z", true)]
        [RegisterBinding(typeof(HDRPMaterialPropertyThicknessRemap), "Value.w", true)]
        public float4 Value;

        class HDRPMaterialPropertyThicknessRemapBaker : Baker<HDRPMaterialPropertyThicknessRemapAuthoring>
        {
            public override void Bake(HDRPMaterialPropertyThicknessRemapAuthoring authoring)
            {
                HDRPMaterialPropertyThicknessRemap component = default(HDRPMaterialPropertyThicknessRemap);
                component.Value = authoring.Value;
                AddComponent(component);
            }
        }
    }
}
#endif
