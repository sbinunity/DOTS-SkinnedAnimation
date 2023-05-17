
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Deformations;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

[RequireMatchingQueriesForUpdate]
[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateBefore(typeof(DeformationsInPresentation))]
[BurstCompile]
public partial struct SkinAnimatingSystem : ISystem
{
    int frame, frame2;
    float dt;

    int clipIndex;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

        Debug.Log("OnCreate");

        clipIndex = 0;
        frame=frame2= 0;
        dt= 0;
       // throw new System.NotImplementedException();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
       // throw new System.NotImplementedException();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
       
        // Debug.Log("OnUpdate");

        if (  Input.GetKeyDown(KeyCode.Space))
        {
            clipIndex = (++clipIndex) % 2;

        }

        dt += SystemAPI.Time.DeltaTime;

        if(dt>=0.0166667f)
        {

            state.Dependency = new SetupSkinMatrixJob()
            {

                clipIndex = 0,
                frameIndex = (++frame) % 201,

            }.ScheduleParallel(state.Dependency);


            state.Dependency = new SetupSkinMatrixJob()
            {
                clipIndex = 1,
                frameIndex = (++frame2) % 40,

            }.ScheduleParallel(state.Dependency);
            dt = 0;

        }



    }

   


    [BurstCompile]
    private partial struct SetupSkinMatrixJob : IJobEntity
    {
     

        public int frameIndex;
        public int clipIndex;

        [BurstCompile]
        public void Execute(ref DynamicBuffer<SkinMatrix> skinMatrices, ref  DynamicBuffer<SkinMatrixData> matrixData, in PlayerComponent p)
        {
            if (  p.id %2  == clipIndex)
            {
                for (int i = 0; i < skinMatrices.Length; i++)
                {
                    var m = matrixData[clipIndex].animationBlob.Value.frameMatrices[frameIndex][i];

                    SkinMatrix sm = new SkinMatrix();
                    sm.Value = new float3x4(m.c0.xyz, m.c1.xyz, m.c2.xyz, m.c3.xyz);

                    skinMatrices[i] = sm;
                }
            }
        }
    }
}

