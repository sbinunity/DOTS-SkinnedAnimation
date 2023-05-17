using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayers : MonoBehaviour
{

    public Transform prefab;
    public int count = 1000;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab).position = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
