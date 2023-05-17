using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.IO;

public class BakeAnimations : EditorWindow
{


    static BakeAnimations window;


    AnimationClip clip;
    GameObject go;

    float sampleTime;
    float sampleStep = 1.0f / 30;
    [MenuItem("Sbin/ BakeAnimation")]
    static void Init()
    {
        window = EditorWindow.CreateInstance<BakeAnimations>();
        window.Show();
    }

    private void OnGUI()
    {
        clip = (AnimationClip)EditorGUILayout.ObjectField(clip, typeof(AnimationClip), false);


        go=(GameObject) EditorGUILayout.ObjectField(go, typeof(GameObject), true);

        if(GUILayout.Button("SampleAnimation"))
        {

           //EditorCoroutine.Start(  SampleAllFramesAnimation() );

            SampleAllFramesAnimation();
        }

        if (GUILayout.Button("BakedMesh"))
        {

            BakedMesh();
        

        }

        if (GUILayout.Button("Test Encoding&Decoding"))
        {

            float v = -0.99999f;// -0.992345f;
            Debug.Log("原值=" + v);

            Vector4 encode = EncodeFloatRGBA(v);

            Debug.Log("编码后=" + encode);

            float r = DecodeFloatRGBA(encode);

            Debug.Log("解码后=" + r);

        }

    }

    void SampleAllFramesAnimation()
    {


        var skins = go.GetComponentsInChildren<SkinnedMeshRenderer>();


        foreach (var skin in skins)
        {
            float time = 0;
           
            List<Matrix4x4[]> list = new List<Matrix4x4[]>();

            while (time < clip.length)
            {
                EditorUtility.DisplayProgressBar("采样bone", "正在采样", time / clip.length);

                var frame = SampleSingleFrameAnimation(skin, time);
                list.Add(frame);
                time += 1.0f / 30;
                // Debug.Log(frame.Length + "   " + time);
            }

            #region 采用序列化存储
            BoneMatrix boneMatrix = ScriptableObject.CreateInstance<BoneMatrix>();
            boneMatrix.Frames = new BoneMatrix.Frame[list.Count];
            for (int i = 0; i < list.Count; i++)
            {

                BoneMatrix.Frame frame = new BoneMatrix.Frame();

                frame.matrix = new Matrix4x4[list[i].Length];

                for (int j = 0; j < list[i].Length; j++)
                {
                    frame.matrix[j] = list[i][j];
                }

                boneMatrix.Frames[i] = frame;
            }

            AssetDatabase.CreateAsset(boneMatrix, Path.Combine("Assets/Samples/BakeBoneMatrix", "animation_"+clip.name + ".asset"));
            AssetDatabase.SaveAssets();

            #endregion 采用序列化存储

           // BakeBoneMap(list, skin.name);


           
        }
        
       
        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();

    }
    Matrix4x4[] SampleSingleFrameAnimation(SkinnedMeshRenderer skin, float time)
    {
        clip.SampleAnimation(go, time);

       
        Matrix4x4[] matrices = new Matrix4x4[skin.bones.Length];

        for (int i = 0; i < skin.bones.Length; i++)
        {
            var bone = skin.bones[i];
            Matrix4x4 m = go.transform.worldToLocalMatrix * bone.localToWorldMatrix * skin.sharedMesh.bindposes[i];

            matrices[i] = m;
        }

        

        return matrices;


    }


    void BakedMesh()
    {
        var skins = go.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var skin in skins)
        {
            Mesh mesh = new Mesh();
            //  skin.BakeMesh(mesh);
            mesh.vertices = skin.sharedMesh.vertices;
            mesh.uv = skin.sharedMesh.uv;
            mesh.triangles = skin.sharedMesh.triangles;
            mesh.normals = skin.sharedMesh.normals;

            Vector2[] uv2 = new Vector2[mesh.vertexCount];
            //  Vector4[] tangents = new Vector4[mesh.vertexCount];
            Color[] colors = new Color[mesh.vertexCount];
            for (int i = 0; i < skin.sharedMesh.boneWeights.Length; i++)
            {
                var weights = skin.sharedMesh.boneWeights[i];
                int r = weights.boneIndex0;
                int g = weights.boneIndex1;
                int b = weights.boneIndex2;
                int a = weights.boneIndex3;

                //  tangents[i] = new Vector4(r, g, b, a);
                uv2[i] = new Vector2(r * 128 + g, b * 128 + a);
                colors[i] = new Color(weights.weight0, weights.weight1, weights.weight2, weights.weight3);

                Debug.Log(weights.weight0+"  "+ weights.weight1+"  "+weights.weight2+"  "+weights.weight3);

            }

            //  mesh.tangents = tangents;
            mesh.colors = colors;
            mesh.uv2 = uv2;
            //Debug.Log(skin.sharedMesh.boneWeights.Length);

            AssetDatabase.CreateAsset(mesh, Path.Combine("Assets/Samples/BakedMesh", skin.name+".mesh"));
            AssetDatabase.SaveAssets();
        }

       
        AssetDatabase.Refresh();

    }



    void BakeBoneMap(List<Matrix4x4[]> frames, string name)
    {

        int width = frames[0].Length;
        int height = frames.Count;

        Debug.Log(width+"       "+height);

        Texture2D boneMap = new Texture2D(width*4,height*4,TextureFormat.RGBA32,false);
        boneMap.filterMode = FilterMode.Point;
        boneMap.wrapMode = TextureWrapMode.Clamp;
        

        for (int h   = 0; h < frames.Count; h++)
        {
            Matrix4x4[] matrix = frames[h];

            for (int w = 0; w < matrix.Length; w++)
            {
                Matrix4x4 m = matrix[w];

                int x = w * 4;
                int y = h * 4;
                boneMap.SetPixel(x, y, EncodeFloatRGBA(m.m00));
                boneMap.SetPixel(x + 1, y, EncodeFloatRGBA(m.m01));
                boneMap.SetPixel(x + 2, y, EncodeFloatRGBA(m.m02));
                boneMap.SetPixel(x + 3, y, EncodeFloatRGBA(m.m03));

                boneMap.SetPixel(x, y + 1, EncodeFloatRGBA(m.m10));
                boneMap.SetPixel(x + 1, y + 1, EncodeFloatRGBA(m.m11));
                boneMap.SetPixel(x + 2, y + 1, EncodeFloatRGBA(m.m12));
                boneMap.SetPixel(x + 3, y + 1, EncodeFloatRGBA(m.m13));

                boneMap.SetPixel(x, y + 2, EncodeFloatRGBA(m.m20));
                boneMap.SetPixel(x + 1, y + 2, EncodeFloatRGBA(m.m21));
                boneMap.SetPixel(x + 2, y + 2, EncodeFloatRGBA(m.m22));
                boneMap.SetPixel(x + 3, y + 2, EncodeFloatRGBA(m.m23));


                boneMap.SetPixel(x, y + 3, EncodeFloatRGBA(m.m30));
                boneMap.SetPixel(x + 1, y + 3, EncodeFloatRGBA(m.m31));
                boneMap.SetPixel(x + 2, y + 3, EncodeFloatRGBA(m.m32));
                boneMap.SetPixel(x + 3, y + 3, EncodeFloatRGBA(m.m33));
            }

           
        }

        boneMap.Apply();

        byte[] data = boneMap.EncodeToTGA();
        System.IO.File.WriteAllBytes(Application.dataPath + $"/GPUSkinning/BakedAnimation/{name}.tga", data);

        AssetDatabase.Refresh();

    }


    /// <summary>
    /// 把float编码到RGBA
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector4 EncodeFloatRGBA(float v)
    {
        v = v * 0.01f + 0.5f;

        if (v < 0) Debug.Log(v);

        Vector4 kEncodeMul =new Vector4(1.0f, 255.0f, 65025.0f, 16581375.0f);
        float kEncodeBit = 1.0f / 255.0f;
        Vector4 enc = kEncodeMul * v;
        for (var i = 0; i < 4; i++)
            enc[i] = enc[i] - Mathf.Floor(enc[i]);
        enc = enc - new Vector4(enc.y, enc.z, enc.w, enc.w) * kEncodeBit;
        return enc;
    }

    /// <summary>
    /// 把RGBA解码到float
    /// </summary>
    /// <param name="enc"></param>
    /// <returns></returns>
     float DecodeFloatRGBA(Vector4 enc)
    {
        Vector4 kDecodeDot = new Vector4(1.0f, 1 / 255.0f, 1 / 65025.0f, 1 / 16581375.0f);
        float v= Vector4.Dot(enc, kDecodeDot);
        v = v * 100 - 50;
        return v;
    }



    #region 采样动画曲线
    //IEnumerator BakeAnimation()
    //{
    //List<Matrix4x4> matrices = new List<Matrix4x4>();

    //float second = 0;
    //float fpsStep = 0.0333333f;

    //EditorCurveBinding[] curvesBinding = AnimationUtility.GetCurveBindings(clip);

    //foreach (var curveBinding in curvesBinding)
    //{

    //    Debug.Log(curveBinding.path+"    "+ curveBinding.propertyName);

    //    //// 旋转

    //    AnimationCurve curve= AnimationUtility.GetEditorCurve(clip, curveBinding);

    //Vector3 translation = new Vector3(curvePX.Evaluate(second), curvePY.Evaluate(second), curvePZ.Evaluate(second));

    //Quaternion rotation = new Quaternion(curve.Evaluate(second), curveRY.Evaluate(second), curveRZ.Evaluate(second), curveRW.Evaluate(second));

    //rotation=  Quaternion.Normalize(rotation);

    //matrices.Add(Matrix4x4.TRS(translation, rotation, Vector3.one));

    //second += fpsStep;

    //}
    //}
    #endregion 采样动画曲线
}


