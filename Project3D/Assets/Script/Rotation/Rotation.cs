using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [Range(0.0f, 180.0f)]
    public float Angle;

    [Range(0.0f, 1.0f)]
    public float dis;

    private Vector3 temp;
    private Vector3 dest;

    private int check;
    private bool move;

    void Start()
    {
        Angle = 0.0f;
        dis = 0.0f;

        temp = new Vector3(0.0f, 0.0f, 0.0f);
        dest = new Vector3(100.0f, 0.0f, 0.0f);
        check = 0;

        move = false;
    }

    void Update()  // 동기 방식: 절차에 따라 실행, 순서대로 돎
    {
        //transform.eulerAngles = new Vector3(0.0f, Angle, 0.0f);


        // 값을 받아옴
        Quaternion rotation = transform.rotation;

        // ** rotation 각의 변경

        //transform.rotation = Quaternion.Lerp(transform.rotation,  );  // 많이 사용

        // 변경된 값 다시 세팅
        transform.rotation = rotation;

        if(Input.GetMouseButtonDown(0))
        {
            //StartCoroutine(SetMove());
            function();
        }
    }

    void function()
    {
        if (move)
            return;

        move = true;
        StartCoroutine(SetMove());
    }

    IEnumerator SetMove()  // 코루틴 함수 - 비동기 방식: 절차 무시하고 따로 돎
    {
        /*
        if (move)
            yield break;

        move = true;
        */

        float time = 0.0f;

        check = (check == 0) ? 1 : 0;

        while(time < 1.0f)
        {
            if(check == 0)
            {
                transform.position = Vector3.Lerp(dest, temp, time);  // 프레임 단위로 도니까 속도 일정
            }
            else
            {
                transform.position = Vector3.Lerp(temp, dest, time);
            }

            time += Time.deltaTime;

            yield return null;
        }

        move = false;  // 비동기는 끝나는 시점을 알 수 없으므로 끝나는 시점을 알기 위해 bool 변수 사용

        /*
        // 많이 사용하는 Lerp 함수 형태
        transform.position = Vector3.Lerp(
            transform.position,
            transform.position * 5.0f,  // 5.0f: Speed
            dis) * Time.deltaTime;  // dis: 비율 - Start와 End의 거리의 dis 비율만큼 이동, Start와 End의 거리가 줄어들면서 점점 느려짐
         */

    }
}
