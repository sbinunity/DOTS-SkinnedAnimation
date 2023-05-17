using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//[ExecuteInEditMode]
public class GPUSkinning : MonoBehaviour
{

    public bool useBoneMatrix = true;

    public BoneMatrix boneMatrix;

    private Material material;

    // Start is called before the first frame update
    void Start()
    {

        material = GetComponent<MeshRenderer>().material;

        // Debug.Log(boneMatrix.Frames.Length);

        //// 使用 Cpu传参
        //  material.SetMatrixArray("_boneMatrix", boneMatrix.Frames[0].matrix);


        // CPUSkinning();
    }

    int frame = 0;
    float dt = 0;
    private void Update()
    {
        dt += Time.deltaTime;
        if (dt >= 1.0f / 30)
        {
            dt = 0;
            if (useBoneMatrix)
                material.SetMatrixArray("_boneMatrix", boneMatrix.Frames[(++frame) % boneMatrix.Frames.Length].matrix);
            else
                material.SetInt("_Frame", (frame++) % 162);

        }
    }

    // Update is called once per frame
    //void CPUSkinning()
    //{
    //    Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
    //    Mesh animesh = new Mesh();

    //    Vector3[] vertices = new Vector3[mesh.vertexCount];


    //    for (int i = 0; i < mesh.vertexCount; i++)
    //    {
    //        int bone0 = (int)(mesh.tangents[i].x * 100);
    //        int bone1 = (int)(mesh.tangents[i].y * 100);
    //        int bone2 = (int)(mesh.tangents[i].z * 100);
    //        int bone3 = (int)(mesh.tangents[i].w * 100);

    //        float w0 = mesh.colors[i].r;
    //        float w1 = mesh.colors[i].g;
    //        float w2 = mesh.colors[i].b;
    //        float w3 = mesh.colors[i].a;

    //        Vector3 p0=  boneMatrix.matrices[bone0].MultiplyPoint(mesh.vertices[i]) * w0;
    //        Vector3 p1 = boneMatrix.matrices[bone1].MultiplyPoint(mesh.vertices[i]) * w1;
    //        Vector3 p2 = boneMatrix.matrices[bone2].MultiplyPoint(mesh.vertices[i]) * w2;
    //        Vector3 p3 = boneMatrix.matrices[bone3].MultiplyPoint(mesh.vertices[i]) * w3;

    //        vertices[i] = p0 + p1 + p2 + p3;


    //    }
    //    animesh.vertices = vertices;
    //    animesh.triangles = mesh.triangles;
    //    animesh.normals = mesh.normals;


    //    GetComponent<MeshFilter>().mesh = animesh;

    //}



}
