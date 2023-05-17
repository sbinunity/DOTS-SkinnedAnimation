using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[BurstCompile]
public partial struct PlayerSpawnerSystem : ISystem
{

  

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
       // throw new System.NotImplementedException();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
       // throw new System.NotImplementedException();
    }

    /// <summary>
    /// adb logcat | findstr com.sbin.dots >D:\crash.txt
    /// </summary>
    /// <param name="state"></param>

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PlayerSpawnerComponent spawner;
       bool b= SystemAPI.TryGetSingleton(out  spawner);
        
        if (!b || spawner.player_entity== Entity.Null) return;


        Unity.Mathematics.Random random = new Unity.Mathematics.Random(1);
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        for (int i = 0; i < spawner.count; i++)
        {
            var e = ecb.Instantiate(spawner.player_entity);
            ecb.SetComponent(e, new PlayerComponent { id =i, p=e });
            ecb.SetComponent(e, new LocalTransform { Position = new float3(random.NextFloat(-30f, 30f), 0, random.NextFloat(-30f, 30f)), Rotation=Quaternion.identity, Scale=1});
            ecb.SetComponent(e,new PlayerComponent { id =i, p=e });
        }
        ecb.Playback(state.EntityManager);



        // foreach (var (t,p) in SystemAPI.Query<TransformAspect,RefRW< PlayerComponent> >())
        //  {
        // t.WorldPosition = new float3(random.NextFloat(-30f, 30f), 0, random.NextFloat(-30f, 30f));
        // p.ValueRW.id = random.NextInt(0,spawner.count);
        //  }


        if (spawner.useLinkedEntityGroup)
        {
            var eq = state.GetEntityQuery(ComponentType.ReadOnly<PlayerComponent>());
            NativeArray<Entity> entityArray = eq.ToEntityArray(Allocator.TempJob);
            foreach (var e in entityArray)
            {
                var buff = state.EntityManager.GetBuffer<LinkedEntityGroup>(e);
                int f = random.NextInt(0, 1812 / 4);
                state.EntityManager.SetComponentData(buff[1].Value, new MaterialAnimationFrame() { Frame = f });
                state.EntityManager.SetComponentData(buff[2].Value, new MaterialAnimationFrame() { Frame = f });
            }
        }

        state.Enabled = false;

    }

}



