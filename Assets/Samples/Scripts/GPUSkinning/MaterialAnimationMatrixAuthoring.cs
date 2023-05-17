
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Entities.Graphics;

namespace Unity.Rendering
{
    [MaterialProperty("_BufferIndexStart")]
    public struct MaterialAnimationMatrix : IComponentData
    {
        // public int part;
        public int BufferIndexStart;
       // public  NativeArray<float4x4> boneMatrix;
    }

   // [UnityEngine.DisallowMultipleComponent]
    public class MaterialAnimationMatrixAuthoring : UnityEngine.MonoBehaviour
    {
        // [Unity.Entities.RegisterBinding(typeof(MaterialAnimationMatrix), "_boneMatrix", true)]
        //public float4x4[] _boneMatrix;
        public int BufferIndexStart;

        class MaterialAnimationMatrixBaker : Unity.Entities.Baker<MaterialAnimationMatrixAuthoring>
        {
            public override void Bake(MaterialAnimationMatrixAuthoring authoring)
            {
                Unity.Rendering.MaterialAnimationMatrix component = default;
               
              //  component.part = authoring.part;
                component.BufferIndexStart = authoring.BufferIndexStart;
                AddComponent(component);
            }
        }
    }
}

