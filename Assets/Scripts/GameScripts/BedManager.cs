using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedManager : MonoBehaviour
{
    public static BedManager instance { get; private set; }
    BedBehaviour[] beds;
    [SerializeField] private AstarPath pathFinder;
    public Rect MapRect { get; private set; }
    private void Awake()
    {
        instance = this;
        var scene = SceneDataManager.instance.selectScene;
        beds = scene.SceneLoad(null);
        var size = new Vector2((scene.sceneInfo.right - scene.sceneInfo.left) * .32f, (scene.sceneInfo.bottom - scene.sceneInfo.top) * .32f);
        MapRect = new Rect(0, -size.y, size.x, size.y);
        pathFinder.data.gridGraph.unclampedSize = size;
        pathFinder.data.gridGraph.center = size * .5f * new Vector2(1, -1);
        pathFinder.data.gridGraph.Scan();
    }
    public Vector3 GetPos(string id)
    {
        return beds[int.Parse(id)].GetPos();
    }
    public string GetID(BedBehaviour bed)
    {
        return Array.IndexOf(beds, bed).ToString();
    }
    public void Sleep(string id, int plays)
    {
        beds[int.Parse(id)].SetType(plays);
    }
    public void GetUP(string id)
    {
        beds[int.Parse(id)].SetType(-1);
    }
}