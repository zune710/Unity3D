using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera Cam;

    [Range(-360.0f, 360.0f)]
    public float CameraRotationX;
    [Range(-360.0f, 360.0f)]
    public float CameraRotationY;

    public float CameraPositionX;
    public float CameraPositionY;
    public float CameraPositionZ;

    private float Speed;
    private Vector3 ClickPos;

    public bool Drag;
    public bool Move;
    public bool check;

    private float OldDistance;

    //private float time;

    void Start()
    {
        Cam = Camera.main;

        CameraRotationX = 5.7f;
        CameraRotationY = 0.0f;

        CameraPositionX = 0.54f;
        CameraPositionY = 1.43f;
        CameraPositionZ = -2.3f;

        Speed = 5.0f;
        ClickPos = Vector3.zero;
        Drag = false;
        Move = false;
        check = false;

        OldDistance = 0.0f;

        //time = 0.0f;
    }

    void Update()
    {
        if (Player.isOpening)
            return;

        if (-20.0f < Cam.transform.eulerAngles.y && Cam.transform.eulerAngles.y < 20.0f)
            Move = true;
        else
            Move = false;

        if (Input.GetMouseButtonDown(1))
        {
            Drag = true;
            ClickPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 direction = (Input.mousePosition - ClickPos).normalized;
            float distanceX = Mathf.Abs(Input.mousePosition.x - ClickPos.x);

            if (OldDistance != distanceX)
                check = true;

            if(check)
            {
                // -20 ~ 20
                CameraRotationY = Cam.transform.rotation.y + direction.x * Time.deltaTime * distanceX * 0.2f;
                OldDistance = distanceX;

                if (360.0f < CameraRotationY || CameraRotationY < -360.0f)
                    CameraRotationY = CameraRotationY % 360.0f;

                if (-20.0f < CameraRotationY && CameraRotationY < 20.0f)
                    Move = true;

                check = false;
            }
               
            Debug.Log("CameraRotationY: " + CameraRotationY);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Drag = false;
            //time = 0.0f;
        }

        Vector3 offset = new Vector3(CameraPositionX, CameraPositionY, CameraPositionZ);

        Cam.transform.position = Vector3.Lerp(
            Cam.transform.position,
            transform.position + offset,
            Time.deltaTime * Speed);

        if(Move)
        {
            Cam.transform.rotation = Quaternion.Lerp(
                Cam.transform.rotation,
                Quaternion.Euler(CameraRotationX, CameraRotationY, 0.0f),
                Time.deltaTime * Speed);

            //time += Time.deltaTime;
        }
    }
}
