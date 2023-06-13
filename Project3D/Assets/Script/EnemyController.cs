using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    public Node Target;

    private float Speed;

    Vector3 LeftCheck;
    Vector3 RightCheck;

    [Range(0.0f, 180.0f)]
    public float Angle;

    private bool move;

    private void Awake()
    {
        SphereCollider coll = GetComponent<SphereCollider>();
        coll.radius = 0.05f;
        coll.isTrigger = true;

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;

        Target = GameObject.Find("ParentObject").transform.GetChild(0).GetComponent<Node>();
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
    }

    private void Update()
    {
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
        //Vector3 LineDir = transform.TransformDirection(Vector3.forward) * 3.0f;

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
                Mathf.Sin((transform.eulerAngles.y + Angle) * Mathf.Deg2Rad), 0.0f, Mathf.Cos((transform.eulerAngles.y + Angle) * Mathf.Deg2Rad)) * 2.5f,
        Color.green);

        if (Physics.Raycast(transform.position, RightCheck, out hit, 5.0f))
        {

        }


        
        
        int Count = (int)((Angle * 2) / 5.0f);

        for(int i = 1; i < Count; ++i)
        {
            Debug.DrawRay(transform.position,
            new Vector3(
                Mathf.Sin((i * 5.0f + startAngle) * Mathf.Deg2Rad), 0.0f, Mathf.Cos((i * 5.0f + startAngle) * Mathf.Deg2Rad)) * 2.5f,
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

    void function()
    {
        if (move)
            return;

        move = true;
        StartCoroutine(SetMove());
    }

    IEnumerator SetMove()
    {
        float time = 0.0f;

        while (time < 1.0f)
        {


            time += Time.deltaTime;

            yield return null;
        }

        move = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        move = false;

        if (Target.transform.name == other.transform.name)
            Target = Target.Next;
    }
}
