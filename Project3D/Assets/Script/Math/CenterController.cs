using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterController : MonoBehaviour
{
    /* // 동그라미
    public List<GameObject> PointList;
    
    void Start()
    {
        for(int i = 0; i < 72; ++i)
        {
            GameObject obj = new GameObject("point");

            obj.AddComponent<MyGizmo>();

            obj.transform.position = new Vector3(
                Mathf.Sin((i * 5.0f) * Mathf.Deg2Rad), // 방향만
                Mathf.Cos((i * 5.0f) * Mathf.Deg2Rad), // 방향만
                0.0f) * 5.0f;  // 5.0f: distance

            PointList.Add(obj);
        }
    }
    */

    // 점프
    [Range(-90.0f, 90.0f)]
    public float Angle;

    private void Start()
    {
        gameObject.AddComponent<MyGizmo>();

        //Angle = 5.0f;
    }

    private void Update()
    {
        /*
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 Start = new Vector3(5.0f, 0.0f, 0.0f);
        Vector3 End = new Vector3(10.0f, 5.0f, 0.0f);

        Debug.DrawLine(Start, End);

        Vector3 movement = new Vector3(hor, 0.0f, 0.0f) * 5.0f * Time.deltaTime;

        float fx = End.x - Start.x;
        float fy = End.y - Start.y;
        
        float distance = Mathf.Sqrt((fx * fx) + (fy * fy));

        if (Start.x < transform.position.x && transform.position.x < End.x)
            transform.position += new Vector3(fx / distance, fy / distance, 0.0f) * hor * 5.0f * Time.deltaTime;
        else
            transform.position += movement;
         */
    }
}
