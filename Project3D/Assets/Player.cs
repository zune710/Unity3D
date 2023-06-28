using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera Cam;
    private Animator Anim;

    [Range(-360.0f, 360.0f)]
    public float CameraRotationX;
    [Range(-360.0f, 360.0f)]
    public float CameraRotationY;

    public float CameraPositionX;
    public float CameraPositionY;
    public float CameraPositionZ;

    public float Speed;
    private float WalkSpeed;
    private float RunSpeed;

    private Vector3 Direction;
    private Vector3 Movement;

    private bool isOpening;

    private void Awake()
    {
        Cam = Camera.main;
        Anim = GetComponent<Animator>();
    }

    void Start()
    {
        CameraRotationX = 5.7f;
        CameraRotationY = 0.0f;

        CameraPositionX = 0.54f;
        CameraPositionY = 1.43f;
        CameraPositionZ = -2.3f;

        WalkSpeed = 2.0f;
        RunSpeed = 5.0f;

        Speed = WalkSpeed;

        isOpening = true;

        //StartCoroutine(Opening());
    }

    private void FixedUpdate()
    {
        //if (isOpening)
        //    return;

        float Hor = Input.GetAxis("Horizontal");
        float Ver = Input.GetAxis("Vertical");

        // Move
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)
            || (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            Direction = new Vector3(Hor, 0.0f, Ver);
            Movement = Direction * Time.deltaTime * Speed;

            transform.position += Movement;

            if (Hor != 0)
            {
                transform.rotation = Quaternion.Euler(
                0.0f, Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg, 0.0f);
            }
        }

        // Walk Anim
        Anim.SetFloat("Move", Mathf.Abs(Hor));

        if (Hor == 0)
            Anim.SetFloat("Move", Mathf.Abs(Ver));

        // Run & Run Anim
        if (Input.GetKey(KeyCode.LeftShift) && (Hor != 0.0f || Ver != 0.0f))
        {
            Speed = RunSpeed;
            Anim.SetBool("Run", true);
        }
        else
        {
            // Walk
            Speed = WalkSpeed;
            Anim.SetBool("Run", false);
        }
    }

    private void LateUpdate()
    {
        //CameraMove();
    }

    private IEnumerator Opening()
    {
        Vector3 startPos = transform.position + new Vector3(0.54f, 2.85f, -7.44f);
        Vector3 endPos = transform.position + new Vector3(0.54f, 1.43f, -2.3f);

        Quaternion startRot = Quaternion.Euler(28.0f, 0.0f, 0.0f);
        Quaternion endRot = Quaternion.Euler(5.7f, 0.0f, 0.0f);

        Cam.transform.position = startPos;
        Cam.transform.rotation = startRot;

        float time = 3.0f;

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

    private void CameraMove()
    {
        Vector3 offset = new Vector3(CameraPositionX, CameraPositionY, CameraPositionZ);

        Cam.transform.position = Vector3.Lerp(
            Cam.transform.position,
            transform.position + offset,
            Time.deltaTime * 5.0f);

        Cam.transform.rotation = Quaternion.Euler(CameraRotationX, CameraRotationY, 0.0f);
    }
}
