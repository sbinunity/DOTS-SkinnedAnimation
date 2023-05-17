using Unity.Entities;
using UnityEngine.Rendering;

namespace Unity.Rendering
{
    [MaterialProperty("unity_SHCoefficients")]
    internal struct BuiltinMaterialPropertyUnity_SHCoefficients : IComponentData
    {
        public SHCoefficients Value;
    }

    [UnityEngine.DisallowMultipleComponent]
    internal class BuiltinMaterialPropertyUnity_SHCoefficientsAuthoring : UnityEngine.MonoBehaviour
    {
        [RegisterBinding(typeof(BuiltinMaterialPropertyUnity_SHCoefficients), "Value")]
        public SHCoefficients Value;

        class BuiltinMaterialPropertyUnity_SHCoefficientsBaker : Baker<BuiltinMaterialPropertyUnity_SHCoefficientsAuthoring>
        {
            public override void Bake(BuiltinMaterialPropertyUnity_SHCoefficientsAuthoring authoring)
            {
                BuiltinMaterialPropertyUnity_SHCoefficients component = default(BuiltinMaterialPropertyUnity_SHCoefficients);
                component.Value = authoring.Value;
                AddComponent(component);
            }
        }
    }
}
