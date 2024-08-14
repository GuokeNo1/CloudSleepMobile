using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ElementManager : ScriptableObject
{   
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void InitPackages()
    {
        DirectoryInfo dirInfo = new DirectoryInfo($"{Application.persistentDataPath}/packages/");
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
        var dirs = dirInfo.GetDirectories();
        if (dirs.Length == 0)
        {
            PackageCompression.ExtrPackages();
        }
    }
    public static ElementManager[] DetectionPackages()
    {
        DirectoryInfo dirInfo = new DirectoryInfo($"{Application.persistentDataPath}/packages/");
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
        var dirs = dirInfo.GetDirectories();
        if(dirs.Length == 0)
        {
            PackageCompression.ExtrPackages();
            return DetectionPackages();
        }
        List<ElementManager> managers = new List<ElementManager>();
        for(int i = 0; i < dirs.Length; i++)
        {
            var dir = dirs[i];
            var manager = CreateManager(dir);
            if(manager != null)
            {
                managers.Add(manager);
            }
        }
        return managers.ToArray();
    }
    public static ElementManager CreateManager(DirectoryInfo dir)
    {
        ElementManager manager = CreateInstance<ElementManager>();
        manager.name = dir.Name;
        manager.source = dir;
        var infos = dir.GetFiles("*.cloudpack");
        if(infos.Length > 0)
        {
            string infoJson = File.ReadAllText(infos[0].FullName);
            manager.info = JsonUtility.FromJson<PackageInfo>(infoJson);
        }
        else
        {
            return null;
        }
        var files = dir.GetFiles("*.json",SearchOption.AllDirectories);
        for(int i = 0; i < files.Length; i++)
        {
            var filename = files[i].Name.ToLower();
            switch (filename)
            {
                case "beds.json":
                    var bjson = File.ReadAllText(files[i].FullName);
                    manager.beds = JsonUtility.FromJson<ElementList<Bed>>(bjson);
                    break;
                case "sleepers.json":
                    var sjson = File.ReadAllText(files[i].FullName);
                    manager.sleepers = JsonUtility.FromJson<ElementList<Sleeper>>(sjson);
                    break;
                case "scene.json":
                    var sceneJson = File.ReadAllText(files[i].FullName);
                    manager.sceneInfo = JsonUtility.FromJson<SceneDate>(sceneJson);
                    break;
                case "backgrounds.json":
                    var bajson = File.ReadAllText(files[i].FullName);
                    manager.backgrounds = JsonUtility.FromJson<ElementList<Element>>(bajson);
                    break;
                case "decorates.json":
                    var djson = File.ReadAllText(files[i].FullName);
                    manager.decorates = JsonUtility.FromJson<ElementList<Decorate>>(djson);
                    break;
                case "TextboxPlaceHolders.json":
                    break;
            }
        }
        return manager;
    }
    private DirectoryInfo source;
    [SerializeField]
    public PackageInfo info;
    [SerializeField]
    public ElementList<Bed> beds;
    [SerializeField]
    public ElementList<Sleeper> sleepers;
    [SerializeField]
    public ElementList<Decorate> decorates;
    [SerializeField]
    public ElementList<Element> backgrounds;
    [SerializeField]
    public SceneDate sceneInfo;
    public void Load()
    {
        beds.Load(source);
        sleepers.Load(source);
        decorates.Load(source);
        backgrounds.Load(source);
    }
    public BedBehaviour[] SceneLoad(Transform parent)
    {
        Load();
        sceneInfo.CreateBackgrounds(parent, backgrounds.materials);
        sceneInfo.CreateDecorates(parent, decorates.materials);
        return sceneInfo.CreateBeds(parent, beds.materials);
        /*
        for(int i = 0; i < sceneInfo.backgrounds.Length; i++)
        {
            var background = sceneInfo.backgrounds[i];
            var item = backgrounds.materials[background.materialId];
            var obj = item.CreateInstance();
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Background";
            obj.transform.position = sceneInfo.GetPos(background);
            obj.transform.SetParent(parent);
        }
        for (int i = 0; i < sceneInfo.decorates.Length; i++)
        {
            var decorate = sceneInfo.decorates[i];
            var item = decorates.materials[decorate.materialId];
            var obj = item.CreateInstance();
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Other";
            obj.transform.position = sceneInfo.GetPos(decorate);
            obj.transform.SetParent(parent);
        }
        for (int i = 0; i < sceneInfo.beds.Length; i++)
        {
            var bed = sceneInfo.beds[i];
            var item = beds.materials[bed.materialId];
            var obj = item.CreateInstance();
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Other";
            obj.transform.position = sceneInfo.GetPos(bed);
            obj.transform.SetParent(parent);
        }
        */
    }
}
[System.Serializable]
public class PackageInfo
{
    public string mainclient_howtoget;
    public string description;
    public string ipport;
    public string guid;
    public string mainclient;
}
