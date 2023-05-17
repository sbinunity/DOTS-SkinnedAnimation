using System.Diagnostics;
using Unity.Entities;
using Unity.Rendering;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine.PlayerLoop;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public partial struct FrameSystem : ISystem
{

   public  float dt;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        dt = 0;
     
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
       // throw new System.NotImplementedException();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        dt += SystemAPI.Time.DeltaTime;
        if (dt > 0.0166666f)
        {
            //Entities.ForEach((ref MaterialAnimationFrame fc) => {
            //    fc.Frame = (++fc.Frame) % (1812 / 4);
            //    //  UnityEngine.Debug.Log("-----" + fc.frame);
            //}).Schedule();
            
            //==========================================================================
            //foreach ( var fc in SystemAPI.Query<RefRW< MaterialAnimationFrame>>())
            //{
            //    fc.ValueRW.Frame = (++fc.ValueRW.Frame) % (1812 / 4);
            //}

            //=================================================================

            new AnimationJob{  }.ScheduleParallel();

            dt = 0;
           

        }
    }

 
}
[BurstCompile]
public partial struct AnimationJob : IJobEntity
{
   
    // public RefRW<MaterialAnimationFrame> fc;
    [BurstCompile]
    public void Execute(ref MaterialAnimationFrame fc)
    {
        //fc.ValueRW.Frame = (++fc.ValueRW.Frame) % (1812 / 4);
        fc.Frame = (++fc.Frame) % (1812 / 4);
    }
}