
using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_Frame")]
    public struct MaterialAnimationFrame : IComponentData
    {
       // public int part;
        public float Frame;
    }

   // [UnityEngine.DisallowMultipleComponent]
    public class MaterialAnimationFrameAuthoring : UnityEngine.MonoBehaviour
    {
        //[Unity.Entities.RegisterBinding(typeof(MaterialAnimationFrame), "_Frame", true)]

       // public int part;
        public float Frame;



        //private void OnEnable()
        //{
        //    Frame = UnityEngine.Random.Range(0, 1812 / 4);
        //}


        class MaterialAnimationFrameBaker : Unity.Entities.Baker<MaterialAnimationFrameAuthoring>
        {
            public override void Bake(MaterialAnimationFrameAuthoring authoring)
            {
                Unity.Rendering.MaterialAnimationFrame component = default;
               
              //  component.part = authoring.part;
                component.Frame = authoring.Frame;
                AddComponent(component);
            }
        }
    }
}

