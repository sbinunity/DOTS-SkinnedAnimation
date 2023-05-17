using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ScriptableObject 
/// </summary>
public class BoneMatrix : ScriptableObject
{
   
    public Frame[] Frames;

    [System.Serializable]
    public class Frame 
    {
        [SerializeField]
        public Matrix4x4[] matrix;
    }

}



