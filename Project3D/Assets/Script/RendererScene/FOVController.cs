using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class FOVController : MonoBehaviour
{
    void Start()
    {

        Mesh mesh = new Mesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = mesh;

        //Vector3[] vertices = new Vector3[4];  //▽

        //vertices[0] = Vector3.zero;
        //vertices[1] = new Vector3(-2.0f, 0.0f, 2.0f);
        //vertices[2] = new Vector3(0.0f, 0.0f, 2.0f);
        //vertices[3] = new Vector3(2.0f, 0.0f, 2.0f);

        //int[] triangles = new int[6];

        //triangles[0] = 0;
        //triangles[1] = 1;
        //triangles[2] = 2;
        //triangles[3] = 0;
        //triangles[4] = 2;
        //triangles[5] = 3;

        List<Vector3> vertices = new List<Vector3>();

        float Angle = 0.0f;

        vertices.Add(Vector3.zero);

        for (int i = 0; i < 72; ++i)  // 360/5 = 72
        {
            vertices.Add(
                new Vector3(
                    Mathf.Sin(Angle * Mathf.Deg2Rad),
                    0.0f,
                    Mathf.Cos(Angle * Mathf.Deg2Rad)) * 10.0f);  // Z방향부터 돌게 하기 위해 Cos, Sin 위치를 서로 바꿈

            Angle = i * 5.0f;
        }
        // 기준점 zero까지 합치면 vertices.Count는 73


        //int max = (vertices.Count - 2) * 3;

        //int[] triangles = new int[max];

        int[] triangles = new int[(vertices.Count - 1) * 3];

        for (int i = 0; i < vertices.Count - 2; ++i)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        triangles[71 * 3] = 0;
        triangles[71 * 3 + 1] = 71 + 1;
        triangles[71 * 3 + 2] = 1;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
    }

    void Update()
    {

    }
}
