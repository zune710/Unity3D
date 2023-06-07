#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPoint))]
public class WayPointEditor : EditorWindow  // ������ â���� �۾��� ���
{
    [MenuItem("CustomEditor/WayPoint")]  // �޴� �̸�
    public static void ShowWindows()
    {
        GetWindow<WayPointEditor>("WayPoint");  // ���� â�� �̸�
    }

    //[Header("�θ� Node (Group)")]  // �� �� - ã�ƺ���
    //[Tooltip("�θ� Node (Group)")]  // �� �� - ã�ƺ���
    private GameObject Parent;

    private void OnGUI()  // 
    {
        Parent = (GameObject)EditorGUILayout.ObjectField("Parent", Parent, typeof(GameObject), true);

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Add Node", GUILayout.Width(350), GUILayout.Height(25)))
            {
                if (Parent != null)
                {
                    GameObject NodeObject = new GameObject("Node");
                    Node node = NodeObject.AddComponent<Node>();

                    // ** Node�� ����

                    //��忬��



                    NodeObject.transform.SetParent(Parent.transform);

                    NodeObject.transform.position = new Vector3(
                        Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
                }
            }

            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }
}
#endif

/*
[CustomEditor(typeof(WayPoint))]
public class WayPointEditor : Editor  // �ν����ͺ信�� �۾��� ���
{
    public Object Parent;

    public override void OnInspectorGUI()  // �ν����Ͱ� ���ŵ� ������ �ٲ�� ����
    {
        base.OnInspectorGUI();

        WayPoint Target = (WayPoint)target;

        Parent = EditorGUILayout.ObjectField("Parent", Parent, typeof(GameObject), true);

        GUILayout.BeginHorizontal();  // �� �������
        {
            GUILayout.FlexibleSpace();  // ��� ����
    
            if (GUILayout.Button("Button", GUILayout.Width(250), GUILayout.Height(25), GUILayout.MaxWidth(350), GUILayout.MaxHeight(25)))
            {
                if(Parent != null)
                {
    
                }
            }
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }
}
*/
