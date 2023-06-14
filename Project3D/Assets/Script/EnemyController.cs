using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{

    const int T = 1;  // Transform
    const int R = 2;  // Rotation
    const int S = 3;  // Scale
    const int M = 0;  // Matrix


    public Node Target = null;

    public List<Vector3> vertices = new List<Vector3>();

    private float Speed;

    public Material material;

    Vector3 LeftCheck;
    Vector3 RightCheck;

    [Range(0.0f, 180.0f)]
    public float Angle;

    private bool move;

    [Range(1.0f, 2.0f)]
    public float scale;

    private void Awake()
    {
        SphereCollider coll = GetComponent<SphereCollider>();
        coll.radius = 0.05f;
        coll.isTrigger = true;

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;

        //Target = GameObject.Find("ParentObject").transform.GetChild(0).GetComponent<Node>();
    }

    private void Start()
    {
        Speed = 5.0f;

        float x = 2.5f;
        float z = 3.5f;

        LeftCheck = transform.position + (new Vector3(-x, 0.0f, z));
        RightCheck = transform.position + (new Vector3(x, 0.0f, z));

        Angle = 45.0f;

        move = false;

        scale = 1.0f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                MeshFilter meshFilter = hit.transform.gameObject.GetComponent<MeshFilter>();

                Vector3[] verticesPoint = meshFilter.mesh.vertices;

                List<Vector3> temp = new List<Vector3>();

                for(int i = 0; i < verticesPoint.Length; ++i)
                {
                    // Cube
                    /*
                    if (!temp.Contains(verticesPoint[i]) && verticesPoint[i].y < transform.position.y)
                    {
                        temp.Add(verticesPoint[i]);
                    }
                    */
                    
                    // Sphere
                    if (!temp.Contains(verticesPoint[i]) 
                        && verticesPoint[i].y < transform.position.y + 0.05f 
                        && transform.position.y < verticesPoint[i].y + 0.05f)
                    {
                        temp.Add(verticesPoint[i]);
                    }
                }

                for (int i = 0; i < temp.Count; ++i)
                {
                    temp[i] = new Vector3(
                            temp[i].x,
                            0.1f,
                            temp[i].z);
                }

                vertices.Clear();  // vertices에 값이 중복으로 계속해서 들어가는 것을 막기 위한 것
                for (int i = 0; i < temp.Count; ++i)
                {
                    GameObject obj = new GameObject(i.ToString());

                    Matrix4x4[] matrix = new Matrix4x4[4];
                    matrix[T] = Matrix.Translate(hit.transform.position);
                    matrix[R] = Matrix.Rotate(hit.transform.eulerAngles);
                    matrix[S] = Matrix.Scale(hit.transform.lossyScale * scale);
                    // Matrix4x4.Translate() 등 만들어진 것 있음, 위의 함수는 기존에 있는 함수를 직접 만들어본 것

                    matrix[M] = matrix[T] * matrix[R] * matrix[S];  // T, R, S 순으로 곱해줘야 함

                    Vector3 v = matrix[M].MultiplyPoint(temp[i]);
                    
                    vertices.Add(v);

                    obj.transform.position = v;
                    obj.AddComponent<MyGizmo>();
                }
            }
        }

        if (Target)
        {
            Vector3 Direction = (Target.transform.position - transform.position).normalized;

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(Direction),
                0.016f);

            if (move)
            {
                transform.position += Direction * Speed * Time.deltaTime;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(Direction),
                Time.deltaTime);

                Vector3 targetDir = Target.transform.position - transform.position;

                if (Vector3.Angle(targetDir, transform.forward) < 0.5f)
                    move = true;
            }
        }
    }

    private void FixedUpdate()
    {
        float startAngle = (transform.eulerAngles.y - Angle);

        RaycastHit hit;

        Debug.DrawRay(transform.position,
            new Vector3(
                Mathf.Sin(startAngle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(startAngle * Mathf.Deg2Rad)) * 2.5f,
            Color.white);

        if (Physics.Raycast(transform.position, LeftCheck, out hit, 5.0f))
        {

        }

        Debug.DrawRay(transform.position,
            new Vector3(
                Mathf.Sin((transform.eulerAngles.y + Angle) * Mathf.Deg2Rad), 
                0.0f, 
                Mathf.Cos((transform.eulerAngles.y + Angle) * Mathf.Deg2Rad)) * 2.5f,
            Color.green);

        if (Physics.Raycast(transform.position, RightCheck, out hit, 5.0f))
        {

        }
        

        int Count = (int)((Angle * 2) / 5.0f);

        for(int i = 1; i < Count; ++i)
        {
            Debug.DrawRay(transform.position,
                new Vector3(
                    Mathf.Sin((i * 5.0f + startAngle) * Mathf.Deg2Rad), 
                    0.0f, 
                    Mathf.Cos((i * 5.0f + startAngle) * Mathf.Deg2Rad)) * 2.5f,
                Color.red);
        }
        // ↑ 동일 ↓
        /*
        for (float f = startAngle + 5.0f; f < (transform.eulerAngles.y + Angle - 5.0f); f += 5.0f)
        {
            Debug.DrawRay(transform.position,
                new Vector3(
                    Mathf.Sin(f * Mathf.Deg2Rad), 0.0f, Mathf.Cos(f * Mathf.Deg2Rad)) * 2.5f,
                Color.red);
        }
         */
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Target)
        {
            if (Target.transform.name == other.transform.name)
            {
                move = false;
                Target = Target.Next;
            }
        }
    }

    void Output(Matrix4x4 _m)
    {
        Debug.Log("=====================================");
        Debug.Log(_m.m00 + ", " + _m.m01 + ", " + _m.m02 + ", " + _m.m03);
        Debug.Log(_m.m10 + ", " + _m.m11 + ", " + _m.m12 + ", " + _m.m13);
        Debug.Log(_m.m20 + ", " + _m.m21 + ", " + _m.m22 + ", " + _m.m23);
        Debug.Log(_m.m30 + ", " + _m.m31 + ", " + _m.m32 + ", " + _m.m33);
    }
}
