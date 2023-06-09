using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Line
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;
}


public class LineCollision : MonoBehaviour
{
    public List<Line> LineList = new List<Line>();

    [SerializeField] private float Width;
    [SerializeField] private float Height;

    void Start()
    {
        Vector3 OldPoint = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < 20; ++i)
        {
            Line line = new Line();

            line.StartPoint = OldPoint;

            /*
            float fY = 0.0f;
            
            while (true)
            {
                fY = Random.Range(-5.0f, 5.0f);
                if (fY != 0.0f) break;
            }

            OldPoint = new Vector3(
                OldPoint.x + Random.Range(1.0f, 5.0f),
                OldPoint.y + fY,
                0.0f);
            */

            OldPoint = new Vector3(
                OldPoint.x + Random.Range(1.0f, 5.0f),
                OldPoint.y + Random.Range(-5.0f, 5.0f),
                0.0f);

            line.EndPoint = OldPoint;

            LineList.Add(line);
        }

        Width = 1.0f;
        Height = 1.0f;
    }

    void Update()
    {
        float PosX = 0;
        float PosY = 0;

        foreach(Line element in LineList)
        {
            Debug.DrawLine(element.StartPoint, element.EndPoint, Color.green);
            
            if(element.StartPoint.x <= transform.position.x && transform.position.x <= element.EndPoint.x)
            {
                Width = element.EndPoint.x - element.StartPoint.x;
                Height = element.EndPoint.y - element.StartPoint.y;
                PosX = element.StartPoint.x;
                PosY = element.StartPoint.y;
            }
        }

        float hor = Input.GetAxis("Horizontal");

        transform.position = new Vector3(
             transform.position.x + hor * 5.0f * Time.deltaTime,
            (Height / Width) * (transform.position.x - PosX) + PosY,
            0.0f);
    }
}
