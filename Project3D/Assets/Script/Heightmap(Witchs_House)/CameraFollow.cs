using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera Cam;

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

    public static bool isOpening = true;

    void Start()
    {
        Cam = Camera.main;

        CameraRotationX = 5.7f;
        CameraRotationY = 0.0f;

        CameraPositionX = 0.54f;
        CameraPositionY = 1.43f;
        CameraPositionZ = -2.3f;

        Speed = 5.0f;
        MouseSpeed = 1.5f;

        StartCoroutine(Opening());
    }

    private void FixedUpdate()
    {
        if (isOpening)
            return;

        // Follow Target
        Vector3 offset = new Vector3(CameraPositionX, CameraPositionY, CameraPositionZ);

        Cam.transform.position = Vector3.Lerp(
            Cam.transform.position,
            Target.transform.position + offset,
            Time.deltaTime * Speed);

        // Rotated by Mouse
        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camEuler = Cam.transform.rotation.eulerAngles;

        float x = Cam.transform.eulerAngles.x;

        if (x < 180)
            x = Mathf.Clamp(x, -1.0f, 70.0f);
        else
            x = Mathf.Clamp(x, 335f, 361f);

        // -50.0f ~ 50.0f으로 제한 필요
        //float y = Mathf.Clamp(camEuler.y + (mousePos.x * MouseSpeed), -50.0f, 50.0f);

        Cam.transform.rotation = Quaternion.Euler(x, camEuler.y + (mousePos.x * MouseSpeed), camEuler.z);
    }

    private IEnumerator Opening()
    {
        Vector3 startPos = Target.transform.position + new Vector3(0.54f, 2.85f, -7.44f);
        Vector3 endPos = Target.transform.position + new Vector3(0.54f, 1.43f, -2.3f);

        Quaternion startRot = Quaternion.Euler(28.0f, 0.0f, 0.0f);
        Quaternion endRot = Quaternion.Euler(5.7f, 0.0f, 0.0f);

        Cam.transform.position = startPos;
        Cam.transform.rotation = startRot;

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

            Cam.transform.position = Vector3.Lerp(
                Cam.transform.position, endPos, Time.deltaTime);

            Cam.transform.rotation = Quaternion.Lerp(
                Cam.transform.rotation, endRot, Time.deltaTime);

            time -= Time.deltaTime;

            yield return null;
        }

        isOpening = false;
    }
}
