using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererManager : MonoBehaviour
{
    private Renderer renderer_;

    private const string path = "Legacy Shaders/Transparent/Specular";

    void Start()
    {
        renderer_ = GetComponent<Renderer>();

        //material.SetColor("_Color", Color.green);  // Standard Shader의 Properties에 _Color 있음 - 사용 O
        //material.SetFloat("_Alpha", 0.5f);  // Standard Shader의 Properties에 _Alpha 없음 - 사용 X
    }

    IEnumerator SetColor(Renderer renderer)
    {
        // ** Color 값 변경이 가능한 Shader로 변경
        Material material = new Material(Shader.Find(path));
        renderer.material = material;

        // ** 변경된 Shader의 Color 값들 받아옴
        Color color = renderer.material.color;

        // ** color.a 값이 0.5f보다 큰 경우에만 반복
        while (0.5f < color.a)
        {
            yield return null;

            // ** Alpha(1) -= Time.deltaTime(0.002정도)
            color.a -= Time.deltaTime;

            renderer.material.color = color;
        }
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            if (GetComponent<Renderer>() != null)
            {
                StartCoroutine(SetColor(GetComponent<Renderer>()));
            }
        }
    }
}
