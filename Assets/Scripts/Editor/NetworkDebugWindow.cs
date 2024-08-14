using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NetworkDebugWindow : EditorWindow
{
    string nickname = "";
    string type = "1";
    string uuid = "{94D448BA-AEFA-4C87-A5F8-36F6640E2F67}";
    Vector2Int pos;
    ElementManager[] managers;
    [MenuItem("����/�������")]
    public static void InitWindow()
    {
        GetWindow<NetworkDebugWindow>("�������").Show();
    }
    private void OnGUI()
    {
        GUILayout.Label("��¼���ƣ�����id����������");
        nickname = GUILayout.TextField(nickname);
        uuid = GUILayout.TextField(uuid);
        type = GUILayout.TextField(type);
        if (GUILayout.Button("��¼"))
        {
            var nwm = GameObject.FindObjectOfType<NetWorkManager>();
            //Debug.Log("login:"+nwm.Connect("localhost:1111", uuid, nickname, type));
        }
        GUILayout.Space(5);

        pos = EditorGUILayout.Vector2IntField("λ��", pos);
        if (GUILayout.Button("����"))
        {
            var nwm = GameObject.FindObjectOfType<NetWorkManager>();
            nwm.Send("pos", pos.x.ToString(), pos.y.ToString());
        }
        if (GUILayout.Button("�ƶ�"))
        {
            var nwm = GameObject.FindObjectOfType<NetWorkManager>();
            nwm.Send("move",pos.x.ToString(),pos.y.ToString());
        }
    }
}
