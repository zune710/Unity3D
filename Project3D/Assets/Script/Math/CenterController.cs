using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterController : MonoBehaviour
{
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
}
