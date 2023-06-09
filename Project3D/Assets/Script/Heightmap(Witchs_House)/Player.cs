using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator Anim;

    [Range(-360.0f, 360.0f)]
    public float CameraRotationX;
    [Range(-360.0f, 360.0f)]
    public float CameraRotationY;

    public float CameraPositionX;
    public float CameraPositionY;
    public float CameraPositionZ;

    private float Speed;
    private float WalkSpeed;
    private float RunSpeed;

    private Vector3 Direction;
    private Vector3 Movement;

    private void Awake()
    {
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
    }

    private void FixedUpdate()
    {
        //if (CameraFollow.isOpening)
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
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.Euler(0.0f, Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg, 0.0f),
                    Time.deltaTime * Speed);
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
}
