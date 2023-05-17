using AnimationSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BakeMatrixAuthoring : MonoBehaviour
{
    public List<BoneMatrix> boneMatrix;

}

public struct SkinMatrixData:IBufferElementData
{
    /// 不能使用 class 类型
    // public BoneMatrix boneMatrix;


    ///blob asset
    public BlobAssetReference<FrameMatrix> animationBlob;


}
public struct FrameMatrix
{
    public BlobArray< BlobArray<float4x4>> frameMatrices;
}


public class BakeMatrixAuthoringBaker : Baker<BakeMatrixAuthoring>
{
    public override void Bake(BakeMatrixAuthoring authoring)
    {
       

        var buffer = AddBuffer<SkinMatrixData>();

        buffer.ResizeUninitialized(authoring.boneMatrix.Count);

        
        for (int b = 0; b < authoring.boneMatrix.Count; b++)
        {
            var blobBuilder = new BlobBuilder(Allocator.Temp);
            ref FrameMatrix animationBlob = ref blobBuilder.ConstructRoot<FrameMatrix>();

            BlobBuilderArray<BlobArray<float4x4>> frameBuilder = blobBuilder.Allocate(ref animationBlob.frameMatrices,
                 authoring.boneMatrix[b].Frames.Length);

            int index = 0;
            foreach (var m in authoring.boneMatrix[b].Frames)
            {

                BlobBuilderArray<float4x4> matrixArrayBuilder = blobBuilder.Allocate(ref frameBuilder[index++],
                     m.matrix.Length
                    );

                for (int i = 0; i < m.matrix.Length; i++)
                {
                    matrixArrayBuilder[i] = ConvertMatrix(m.matrix[i]);
                }

            }

            var data = new SkinMatrixData()
            {
                animationBlob = blobBuilder.CreateBlobAssetReference<FrameMatrix>(Allocator.Persistent)
            };

            buffer[b] = data;
        }

      
    }

    public float4x4 ConvertMatrix(Matrix4x4 m)
    {
        return m.ConvertTo<float4x4>();
    }
}


