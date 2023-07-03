using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera camera;
    private Vector3 direction;
    private Transform target;
    private float distance;

    public LayerMask mask;

    private bool Check;

    private List<RaycastHit> hits = new List<RaycastHit>();

    private const string path = "Legacy Shaders/Transparent/Specular";
    private const string customPath = "Custom/Transparent/MyShader";


    private void Awake()
    {
        //camera = GetComponent<Camera>();
        camera = Camera.main;

        target = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        direction = (target.position - transform.position).normalized;

        distance = Vector3.Distance(target.position, transform.position);

        Check = false;
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + direction * distance, Color.green);  // 월드 좌표
        Ray ray = new Ray(transform.position, direction);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, distance, mask))  // mask: Player Layer만 제외
        {
            if(!hits.Contains(hit))
                hits.Add(hit);



            if (hit.transform != null)
            {
                Check = true;

                Renderer renderer = hit.transform.GetComponent<Renderer>();

                if (renderer != null)
                    StartCoroutine(SetColor(renderer));
            }
            
        }
        else
        {
            Check = false;

            //if (renderer != null)
            //    StartCoroutine(ReColor(renderer));
        }
    }

    IEnumerator SetColor(Renderer renderer)
    {
        // ** Color 값 변경이 가능한 Shader로 변경
        //Material material = new Material(Shader.Find(path));
        //renderer.material = material;
        renderer.material.shader = Shader.Find(customPath);

        // ** 변경된 Shader의 Color 값들 받아옴
        Color color = renderer.material.color;

        // ** color.a 값이 0.5f보다 큰 경우에만 반복
        while (0.5f < color.a)
        {
            yield return null;

            // ** Alpha(1) -= Time.deltaTime(0.002정도)
            // ** Time.deltaTime
            color.a -= Time.deltaTime;

            renderer.material.color = color;
        }
    }

    IEnumerator ReColor(Renderer renderer)
    {
        // ** 변경된 Shader의 Color 값들 받아옴
        Color color = renderer.material.color;

        // ** color.a 값이 0.5f보다 큰 경우에만 반복
        while (color.a < 1.0f)
        {
            yield return null;

            // ** Alpha(1) -= Time.deltaTime(0.002정도)
            // ** Time.deltaTime
            color.a += Time.deltaTime;

            renderer.material.color = color;
        }

        renderer.material.shader = Shader.Find("Standard");
    }
}
