using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 3000;
    }

    private float dt;

    private int FPSCount = 0;

    private string fpsString = "";
    // Update is called once per frame
    void Update()
    {
        FPSCount++;
        dt += Time.deltaTime;
        if (dt >= 1.0f)
        {
            fpsString = "FPS:" + FPSCount;
            FPSCount = 0;
            dt = 0;
        }
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.color = Color.red;

        GUI.Label(new Rect(50, 50, 300, 100), fpsString);
    }
}
