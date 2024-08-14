using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PackageDebugWindow : EditorWindow
{
    ElementManager[] managers;
    [MenuItem("工具/测试场景包")]
    public static void InitWindow()
    {
        GetWindow<PackageDebugWindow>("场景测试").Show();
    }
    private void OnGUI()
    {
        if (GUILayout.Button("读取本地包"))
        {
            managers = ElementManager.DetectionPackages();
        }
        GUILayout.Space(10);
        if (managers != null)
        {
            foreach (ElementManager manager in managers)
            {
                EditorGUILayout.ObjectField(manager, typeof(ElementManager));
                if (GUILayout.Button("加载数据"))
                {
                    manager.Load();
                }
                if (GUILayout.Button("加载场景"))
                {
                    manager.SceneLoad(Selection.activeTransform);
                }
            }
        }
    }
}
