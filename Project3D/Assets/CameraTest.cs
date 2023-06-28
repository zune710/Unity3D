using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    private Camera Cam;

    [Range(-360.0f, 360.0f)]
    public float CameraRotationX;
    [Range(-360.0f, 360.0f)]
    public float CameraRotationY;

    public float CameraPositionX;
    public float CameraPositionY;
    public float CameraPositionZ;

    void Start()
    {
        Cam = Camera.main;

        CameraPositionX = 0.54f;
        CameraPositionY = 1.43f;
        CameraPositionZ = -2.3f;
    }

    void Update()
    {

        Vector3 offset = new Vector3(CameraPositionX, CameraPositionY, CameraPositionZ);

        Cam.transform.position = Vector3.Lerp(
            Cam.transform.position,
            transform.position + offset,
            Time.deltaTime * 5.0f);

        Cam.transform.rotation = Quaternion.Euler(CameraRotationX, CameraRotationY, 0.0f);
    }
}
