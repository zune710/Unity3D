using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject Target;

    [Range(-360.0f, 360.0f)]
    public float CameraRotationX;
    [Range(-360.0f, 360.0f)]
    public float CameraRotationY;

    public float CameraPositionX;
    public float CameraPositionY;
    public float CameraPositionZ;

    private float Speed;
    private float MouseSpeed;
    private float ScrollSpeed;

    private float MoveX;
    private float MoveY;
    private float Distance;
    private float Height;

    public static bool isOpening = true;

    void Start()
    {
        CameraRotationX = 5.7f;
        CameraRotationY = 0.0f;

        CameraPositionX = 0.54f;
        CameraPositionY = 1.43f;
        CameraPositionZ = -2.3f;

        Speed = 5.0f;
        MouseSpeed = 1.5f;
        ScrollSpeed = 100.0f;

        MoveX = 0.0f;
        MoveY = 0.0f;
        Distance = 3.0f;
        Height = 1.0f;

        //StartCoroutine(Opening());
    }

    private void FixedUpdate()
    {
        //if (isOpening)
        //    return;

        //CameraTest();

        CameraUpdate();

    }

    private void CameraUpdate()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        // 마우스의 좌우 이동량을 MoveX 에 누적합니다.
        MoveX += Input.GetAxis("Mouse X");
        MoveY -= Input.GetAxis("Mouse Y");  // MoveX와 달리 빼 줘야 함

        //상하 회전 범위 제한: -10 ~ 50
        MoveY = Mathf.Clamp(MoveY, -10.0f, 50.0f);

        // 마우스 휠로 카메라 거리 조정
        // 기본: 3.0f
        // 범위 제한: 최소 1.0f, 최대 5.0f
        Distance += scrollWheel * Time.deltaTime * ScrollSpeed;
        Distance = Mathf.Clamp(Distance, 1.0f, 5.0f);

        // 이동량에 따라 카메라의 바라보는 방향을 조정합니다.
        transform.rotation = Quaternion.Euler(MoveY, MoveX, 0.0f);

        // 카메라가 바라보는 앞방향은 Z축입니다. 이동량에 따른 Z축 방향의 벡터를 구합니다. ??
        // 카메라 거리 offset 값
        Vector3 distanceOffset = new Vector3(0.0f, 0.0f, Distance);

        // 카메라 높이 offset 값
        Vector3 positionOffset = new Vector3(0.0f, Height, 0.0f);

        // 플레이어의 위치에서 카메라가 바라보는 방향에 벡터값을 적용한 상대 좌표를 차감합니다.
        // 카메라 위치 = 플레이어의 위치 - 카메라 회전각 * (0, 0, 거리)
        // 여기에 positionOffset 을 더해서 카메라 position.y 값 조정
        transform.position = Target.transform.position - (transform.rotation * distanceOffset) + positionOffset;
    }

    private void CameraTest()
    {
        // Follow Target
        Vector3 offset = new Vector3(CameraPositionX, CameraPositionY, CameraPositionZ);

        transform.position = Vector3.Lerp(
            transform.position,
            Target.transform.position + offset,
            Time.deltaTime * Speed);

        // Rotated by Mouse
        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camEuler = transform.rotation.eulerAngles;

        float x = transform.eulerAngles.x;

        if (x < 180)
            x = Mathf.Clamp(x, -1.0f, 70.0f);
        else
            x = Mathf.Clamp(x, 335f, 361f);

        // -50.0f ~ 50.0f으로 제한 필요
        //float y = Mathf.Clamp(camEuler.y + (mousePos.x * MouseSpeed), -50.0f, 50.0f);

        transform.rotation = Quaternion.Euler(x, camEuler.y + (mousePos.x * MouseSpeed), camEuler.z);
    }

    private IEnumerator Opening()
    {
        Vector3 startPos = Target.transform.position + new Vector3(0.54f, 2.85f, -7.44f);
        Vector3 endPos = Target.transform.position + new Vector3(0.54f, 1.43f, -2.3f);

        Quaternion startRot = Quaternion.Euler(28.0f, 0.0f, 0.0f);
        Quaternion endRot = Quaternion.Euler(5.7f, 0.0f, 0.0f);

        transform.position = startPos;
        transform.rotation = startRot;

        float time = 5.0f;

        while (true)
        {
            if (time <= 0)
                break;

            /*
            if (Mathf.Approximately(Cam.transform.position.z, endPos.z)
                && Mathf.Approximately(Cam.transform.rotation.x, endRot.x))
                break;
             */

            transform.position = Vector3.Lerp(
                transform.position, endPos, Time.deltaTime);

            transform.rotation = Quaternion.Lerp(
                transform.rotation, endRot, Time.deltaTime);

            time -= Time.deltaTime;

            yield return null;
        }

        isOpening = false;
    }
}
