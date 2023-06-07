#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPoint))]
public class WayPointEditor : EditorWindow  // 윈도우 창에서 작업할 경우
{
    [MenuItem("CustomEditor/WayPoint")]  // 메뉴 이름
    public static void ShowWindows()
    {
        GetWindow<WayPointEditor>("WayPoint");  // 띄우는 창의 이름
    }

    //[Header("부모 Node (Group)")]  // 안 됨 - 찾아보기
    //[Tooltip("부모 Node (Group)")]  // 안 됨 - 찾아보기
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

                    // ** Node의 설정

                    //노드연결



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
public class WayPointEditor : Editor  // 인스펙터뷰에서 작업할 경우
{
    public Object Parent;

    public override void OnInspectorGUI()  // 인스펙터가 갱신될 때마다 바뀌는 형태
    {
        base.OnInspectorGUI();

        WayPoint Target = (WayPoint)target;

        Parent = EditorGUILayout.ObjectField("Parent", Parent, typeof(GameObject), true);

        GUILayout.BeginHorizontal();  // 줄 띄워쓰기
        {
            GUILayout.FlexibleSpace();  // 가운데 정렬
    
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
