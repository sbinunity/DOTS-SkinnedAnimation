using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    public Entity p;
    public int id;
}

public class PlayerAuthoring:MonoBehaviour
{

    public int id;

    class PlayerBaker: Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            AddComponent(new PlayerComponent() { id= authoring.id, p=GetEntity() });
        }

         
    }
}
