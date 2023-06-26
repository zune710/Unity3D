using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera Cam;
    private Animator Anim;

    public float Speed;
    private float WalkSpeed;
    private float RunSpeed;

    private Vector3 Direction;
    private Vector3 Movement;

    private void Awake()
    {
        Cam = Camera.main;
        Anim = GetComponent<Animator>();
    }

    void Start()
    {
        WalkSpeed = 2.0f;
        RunSpeed = 5.0f;

        Speed = WalkSpeed;
    }

    void Update()
    {
        CameraMove();

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

    private void CameraMove()
    {
        Vector3 offset = new Vector3(0.54f, 1.43f, -2.3f);
        
        Cam.transform.position = Vector3.Lerp(
            Cam.transform.position,
            transform.position + offset,
            Time.deltaTime * 5.0f);

        Cam.transform.rotation = Quaternion.Euler(5.7f, 0.0f, 0.0f);
    }
}
