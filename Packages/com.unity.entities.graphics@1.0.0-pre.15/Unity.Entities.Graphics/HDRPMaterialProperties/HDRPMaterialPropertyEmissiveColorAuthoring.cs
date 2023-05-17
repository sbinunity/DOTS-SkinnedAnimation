#if HDRP_10_0_0_OR_NEWER
using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_EmissiveColor")]
    public struct HDRPMaterialPropertyEmissiveColor : IComponentData { public float3 Value; }

    [UnityEngine.DisallowMultipleComponent]
    public class HDRPMaterialPropertyEmissiveColorAuthoring : UnityEngine.MonoBehaviour
    {
        [RegisterBinding(typeof(HDRPMaterialPropertyEmissiveColor), "Value.x", true)]
        [RegisterBinding(typeof(HDRPMaterialPropertyEmissiveColor), "Value.y", true)]
        [RegisterBinding(typeof(HDRPMaterialPropertyEmissiveColor), "Value.z", true)]
        public float3 Value;

        class HDRPMaterialPropertyEmissiveColorBaker : Baker<HDRPMaterialPropertyEmissiveColorAuthoring>
        {
            public override void Bake(HDRPMaterialPropertyEmissiveColorAuthoring authoring)
            {
                HDRPMaterialPropertyEmissiveColor component = default(HDRPMaterialPropertyEmissiveColor);
                component.Value = authoring.Value;
                AddComponent(component);
            }
        }
    }
}
#endif
