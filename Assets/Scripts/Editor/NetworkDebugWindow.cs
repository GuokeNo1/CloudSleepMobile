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
    [MenuItem("工具/网络测试")]
    public static void InitWindow()
    {
        GetWindow<NetworkDebugWindow>("网络测试").Show();
    }
    private void OnGUI()
    {
        GUILayout.Label("登录名称，场景id，人物类型");
        nickname = GUILayout.TextField(nickname);
        uuid = GUILayout.TextField(uuid);
        type = GUILayout.TextField(type);
        if (GUILayout.Button("登录"))
        {
            var nwm = GameObject.FindObjectOfType<NetWorkManager>();
            //Debug.Log("login:"+nwm.Connect("localhost:1111", uuid, nickname, type));
        }
        GUILayout.Space(5);

        pos = EditorGUILayout.Vector2IntField("位置", pos);
        if (GUILayout.Button("设置"))
        {
            var nwm = GameObject.FindObjectOfType<NetWorkManager>();
            nwm.Send("pos", pos.x.ToString(), pos.y.ToString());
        }
        if (GUILayout.Button("移动"))
        {
            var nwm = GameObject.FindObjectOfType<NetWorkManager>();
            nwm.Send("move",pos.x.ToString(),pos.y.ToString());
        }
    }
}
