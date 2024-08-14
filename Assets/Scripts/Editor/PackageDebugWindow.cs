using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PackageDebugWindow : EditorWindow
{
    ElementManager[] managers;
    [MenuItem("����/���Գ�����")]
    public static void InitWindow()
    {
        GetWindow<PackageDebugWindow>("��������").Show();
    }
    private void OnGUI()
    {
        if (GUILayout.Button("��ȡ���ذ�"))
        {
            managers = ElementManager.DetectionPackages();
        }
        GUILayout.Space(10);
        if (managers != null)
        {
            foreach (ElementManager manager in managers)
            {
                EditorGUILayout.ObjectField(manager, typeof(ElementManager));
                if (GUILayout.Button("��������"))
                {
                    manager.Load();
                }
                if (GUILayout.Button("���س���"))
                {
                    manager.SceneLoad(Selection.activeTransform);
                }
            }
        }
    }
}
