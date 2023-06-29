using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    private Animator Anim;
    
    public float Speed;

    private bool ChangeDir;
    private bool Collision;

    private List<string> shapekeyList = new List<string>
                                            {   "Eyes_Annoyed",
                                                "Eyes_Blink",
                                                "Eyes_Cry",
                                                "Eyes_Dead",
                                                "Eyes_Excited",
                                                "Eyes_Happy",
                                                "Eyes_LookDown",
                                                "Eyes_LookIn",
                                                "Eyes_LookOut",
                                                "Eyes_LookUp",
                                                "Eyes_Rabid",
                                                "Eyes_Sad",
                                                "Eyes_Shrink",
                                                "Eyes_Sleep",
                                                "Eyes_Spin",
                                                "Eyes_Squint",
                                                "Eyes_Trauma",
                                                "Sweat_L",
                                                "Sweat_R",
                                                "Teardrop_L",
                                                "Teardrop_R"
                                            };

    private void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    void Start()
    {
        Anim.Play(shapekeyList[1]);

        //Speed = 3.0f;
        
        ChangeDir = true;
        Collision = false;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(transform.name != "Sparrow")
        {
            if (ChangeDir || Collision)
            {
                float directionX = Random.Range(-1.0f, 1.0f);
                float directionZ = Random.Range(-1.0f, 1.0f);

                StopAllCoroutines();
                StartCoroutine(OnMove(directionX, directionZ));

                ChangeDir = false;

                Anim.SetFloat("Move", Mathf.Abs(directionX));

                if (directionX == 0)
                    Anim.SetFloat("Move", Mathf.Abs(directionZ));
            }
        }
    }

    private IEnumerator OnMove(float dirX, float dirZ)
    {
        float time = 3.0f;

        while (true)
        {
            if (time <= 0)
            {
                ChangeDir = true;
                break;
            }

            transform.position += new Vector3(dirX, 0.0f, dirZ) * Speed * Time.deltaTime;

            transform.rotation = Quaternion.Euler(
                0.0f, Mathf.Atan2(dirX, dirZ) * Mathf.Rad2Deg, 0.0f);

            time -= Time.deltaTime;

            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "WaterCube")  // Fish
        {

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "WaterCube")
            Collision = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "WaterCube")
            Collision = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.name != "Terrain")
            Collision = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.name != "Terrain")
            Collision = false;
    }


}
