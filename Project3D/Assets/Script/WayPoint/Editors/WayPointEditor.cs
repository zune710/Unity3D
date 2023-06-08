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

    //[Header("부모 Node (Group)")]
    //[Tooltip("부모 Node (Group)")]
    private GameObject Parent;

    private void OnGUI()  // 
    {
        EditorGUILayout.LabelField("부모 Node (Group)", EditorStyles.boldLabel);
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
