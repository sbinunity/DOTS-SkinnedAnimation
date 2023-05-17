using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;


public partial struct PlayerSpawnerComponent: IComponentData
{
    public Entity player_entity;
    public int count;
    public bool useLinkedEntityGroup;
    // public Entity e_body;
    //  public Entity e_head;
}

public class PlayerSpawnerAuthoring : MonoBehaviour
{
    public GameObject prefab;

    public int count;

    public bool useLinkedEntityGroup;

    class PlayerSpawnerBaker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            AddComponent(new PlayerSpawnerComponent()
            {
                player_entity = GetEntity(authoring.prefab),
                count=authoring.count,
                useLinkedEntityGroup = authoring.useLinkedEntityGroup
                //  e_body = GetEntity(authoring.prefab.transform.GetChild(0).gameObject),
                //  e_head= GetEntity(authoring.prefab.transform.GetChild(1).gameObject)

            }) ; 
        }
    }
}
