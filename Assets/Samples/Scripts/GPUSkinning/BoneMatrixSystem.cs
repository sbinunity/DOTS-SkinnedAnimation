using System.Diagnostics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.Burst;
using JetBrains.Annotations;
using System.Linq;
using System;

namespace Unity.Rendering
{
    #region SystemBase
    //[UpdateAfter(typeof(EntitiesGraphicsSystem))]
    //[UpdateInGroup(typeof(UpdatePresentationSystemGroup))]
    //partial class BoneMatrixSystem : SystemBase
    //{
    //    int frame, frame2;
    //    public float dt;


    //    protected override void OnCreate()
    //    {

    //        frame = UnityEngine.Random.Range(0, 40);
    //    }


    //    protected override void OnUpdate()
    //    {



    //        dt += SystemAPI.Time.DeltaTime;
    //        if (dt >= 0.0166667f)
    //        {
    //            dt = 0;
    //            frame = (++frame) % 40;
    //            frame2 = (++frame2) % 201;


    //            Matrix4x4[] bufferMatrix = new Matrix4x4[112];


    //            foreach (var buffer in SystemAPI.Query<DynamicBuffer<SkinMatrixData>>())
    //            {
    //                for (int i = 0; i < 56; i++)
    //                {
    //                    var m = buffer[0].animationBlob.Value.frameMatrices[frame][i];
    //                    bufferMatrix[i] = m.ConvertTo<Matrix4x4>();

    //                }

    //                for (int i = 56; i < 112; i++)
    //                {
    //                    var m = buffer[1].animationBlob.Value.frameMatrices[frame2][i-56];
    //                    bufferMatrix[i] = m.ConvertTo<Matrix4x4>();

    //                }
    //                break;
    //            }

    //            Shader.SetGlobalMatrixArray("_boneMatrix", bufferMatrix);


    //            new AnimationJob { }.ScheduleParallel();


    //            //foreach (var ( p,mat) in SystemAPI.Query< PlayerComponent,RefRW < MaterialAnimationMatrix >> ())
    //            // {

    //            //Matrix4x4[] ms = new Matrix4x4[56];
    //            //for (int i = 0; i < 56; i++)
    //            //{
    //            //    var m = buffer[0].animationBlob.Value.frameMatrices[frame][i];
    //            //    ms[i] = m.ConvertTo<Matrix4x4>();
    //            //}
    //            //Shader.SetGlobalMatrixArray("_boneMatrix", ms);
    //            //break;
    //            // }

    //        }
    //    }
    //}

    //[BurstCompile]
    // partial struct AnimationJob : IJobEntity
    //{

    //    // public RefRW<MaterialAnimationFrame> fc;
    //    [BurstCompile]
    //    public void Execute(ref MaterialAnimationMatrix fc, in PlayerComponent p)
    //    {
    //        fc.BufferIndexStart =  (p.id %2) *56;
    //    }
    //}


    #endregion


    #region ISystem Implement
    [BurstCompile]
    [UpdateInGroup(typeof(UpdatePresentationSystemGroup))]
    public partial struct BoneMatrixSystem : ISystem
    {

        int frame1, frame2;
        float dt;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            frame1 = frame2 = 0;
            dt = 0;

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            //throw new System.NotImplementedException();
        }

        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            dt += SystemAPI.Time.DeltaTime;
            if (dt >= 0.0166667f)
            {
                dt = 0;
                frame1 = (++frame1) % 40;
                frame2 = (++frame2) % 201;


                Matrix4x4[] bufferMatrix = new Matrix4x4[112];


                foreach (var buffer in SystemAPI.Query<DynamicBuffer<SkinMatrixData>>())
                {
                    for (int i = 0; i < 56; i++)
                    {
                        var m = buffer[0].animationBlob.Value.frameMatrices[frame1][i];
                        bufferMatrix[i] = m.ConvertTo<Matrix4x4>();

                        var m2 = buffer[1].animationBlob.Value.frameMatrices[frame2][i];
                        bufferMatrix[i+56] = m2.ConvertTo<Matrix4x4>();

                    }

                    //for (int i = 56; i < 112; i++)
                    //{
                    //    var m = buffer[1].animationBlob.Value.frameMatrices[frame2][i - 56];
                    //    bufferMatrix[i] = m.ConvertTo<Matrix4x4>();

                    //}
                    break;
                }

                Shader.SetGlobalMatrixArray("_boneMatrix", bufferMatrix);


                new AnimationJob { }.ScheduleParallel();
            }

        }
    }

    [BurstCompile]
    partial struct AnimationJob : IJobEntity
    {
        [BurstCompile]
        public void Execute(ref MaterialAnimationMatrix fc, in PlayerComponent p)
        {
            fc.BufferIndexStart = (p.id % 2) * 56;
        }
    }


    #endregion
}
