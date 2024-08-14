using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataManager : MonoBehaviour
{
    public ElementManager[] dataManagers { get; private set; }
    public static SceneDataManager instance { get; private set; }
    public ElementManager selectScene;
    public int sleeperMid = 0;
    public string ipport = "";
    public string nickname = "";
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        LeanTween.init(5000);
        DontDestroyOnLoad(gameObject);
        instance = this;
        Application.targetFrameRate = 60;
        Load();
    }
    private void Load()
    {
        dataManagers = ElementManager.DetectionPackages();
        foreach (var manager in dataManagers)
        {
            manager.Load();
        }
        if (dataManagers.Length < 1)
        {

            Load();
        }
    }
}
