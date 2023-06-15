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

    public List<GameObject> vertices = new List<GameObject>();
    public List<GameObject> bestList = new List<GameObject>();
    public List<Node> OpenList = new List<Node>();  // 확인한 노드 저장

    private float Speed;

    public Material material;

    Vector3 LeftCheck;
    Vector3 RightCheck;

    [Range(0.0f, 180.0f)]
    public float Angle;

    private bool move;

    [Range(1.0f, 2.0f)]
    public float scale;

    private GameObject parent;

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
        parent = new GameObject("Nodes");

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

                GameObject startPoint = null;
                float dis = 0.0f;
                float bestDistance = 1000000.0f;

                OpenList.Clear();
                vertices.Clear();  // vertices에 값이 중복으로 계속해서 들어가는 것을 막기 위한 것

                for (int i = 0; i < temp.Count; ++i)
                {
                    GameObject obj = new GameObject(i.ToString());

                    Matrix4x4[] matrix = new Matrix4x4[4];

                    matrix[T] = Matrix.Translate(hit.transform.position);
                    matrix[R] = Matrix.Rotate(hit.transform.eulerAngles);
                    matrix[S] = Matrix.Scale(hit.transform.lossyScale * scale);
                    // Matrix4x4.Translate(), Matrix4x4.Rotate(), Matrix4x4.Scale()

                    matrix[M] = matrix[T] * matrix[R] * matrix[S];  // T, R, S 순으로 곱해줘야 함

                    Vector3 v = matrix[M].MultiplyPoint(temp[i]);
                    dis = Vector3.Distance(transform.position, v);

                    obj.transform.position = v;
                    obj.AddComponent<Node>();
                    obj.tag = "Node";//

                    obj.transform.SetParent(parent.transform);
                    obj.AddComponent<MyGizmo>();

                    if (dis < bestDistance)
                    {
                        bestDistance = dis;
                        startPoint = obj;

                        if(i == 0)
                            vertices.Add(obj);
                    }
                    else
                        vertices.Add(obj);
                }

                if(startPoint)
                {
                    startPoint.GetComponent<MyGizmo>().color = Color.red;
                    OpenList.Add(startPoint.GetComponent<Node>());
                }

                //List<GameObject> CloseList = new List<GameObject>();  // 못 가는 노드 저장
                
                
                Node MainNode = OpenList[0].GetComponent<Node>();  // startPoint
                MainNode.Cost = 0.0f;

                while(vertices.Count != 0)
                {
                    float OldDistance = 1000000.0f;
                    int index = 0;

                    Node node = OpenList[OpenList.Count - 1];//
                    GameObject EndPoint = GameObject.Find("1");//

                    for (int i = 0; i < vertices.Count; ++i)
                    {
                        //float Distance = Vector3.Distance(OpenList[0].transform.position, vertices[i].transform.position);
                        float Distance = Vector3.Distance(node.transform.position, vertices[i].transform.position);

                        if (Distance < OldDistance)
                        {
                            OldDistance = Distance;
                            Node NextNode = vertices[i].GetComponent<Node>();
                            NextNode.Cost = MainNode.Cost + Distance;
                            index = i;
                        }
                    }

                    float prevDis = Vector3.Distance(node.transform.position, EndPoint.transform.position);//
                    float curDis = Vector3.Distance(vertices[index].transform.position, EndPoint.transform.position);//

                    if (!OpenList.Contains(vertices[index].GetComponent<Node>()))
                    {
                        /**/
                        if (curDis < prevDis)
                        {
                            RaycastHit Hit;

                            //Debug.DrawRay(node.transform.position, (vertices[index].transform.position - node.transform.position).normalized, Color.blue, OldDistance);
                            if (Physics.Raycast(node.transform.position, (vertices[index].transform.position - node.transform.position).normalized, out Hit, OldDistance))
                            {
                                if (Hit.transform.tag != "Node")
                                {
                                    vertices.Remove(vertices[index]);
                                    break;
                                }
                                else
                                {
                                    OpenList.Add(vertices[index].GetComponent<Node>());
                                    vertices.Remove(vertices[index]);
                                }
                            }
                            else
                            {
                                OpenList.Add(vertices[index].GetComponent<Node>());
                                vertices.Remove(vertices[index]);
                            }
                        }

                        
                        /*
                        * 조건 1
                        RaycastHit Hit;

                        if (Physics.Raycast(origin(이전노드), direction(현재노드), out Hit, OldDistance))
                        {
                            if (Hit.transform.tag != "Node")
                            {
                                // 장애물이 있어 노드 사용불가
                            }
                            else
                            {
                                // 노드 추가
                            }
                        }
                        */

                        /*
                         *  조건 2
                         *  이전 노드의 위치에서 EndPoint의 거리보다 현재 노드에서 EndPoint의 거리가 더 짧을 때
                         */


                        /*
                        OpenList.Add(vertices[index].GetComponent<Node>());
                        vertices[index].GetComponent<Node>();//?

                        vertices.Remove(vertices[index]);
                         */
                    }

                    if (OpenList[OpenList.Count - 1] == EndPoint.GetComponent<Node>())//
                        break;

                    //bestList[0].GetComponent<Node>().Next;
                    //bestList[1].GetComponent<Node>();
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
