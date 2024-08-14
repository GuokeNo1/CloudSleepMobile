using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneDate
{
    public float left;
    public float right;
    public float top;
    public float bottom;
    public SceneItem[] sleepers;
    public SceneItem[] backgrounds;
    public SceneItem[] decorates;
    public SceneItem[] beds;
    public float defaultBackground = -1;
    public Vector2 GetPos(SceneItem item)
    {
        return new Vector2(item.xPos - left * 32, -(item.yPos - top * 32)) * .01f;
        //return new Vector2(left + item.xPos,top + ((top-bottom) - item.yPos)) * .01f;
    }
    private void DetectionRect(SceneItem[] items,ref Rect rect)
    {
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var pos = GetPos(item);
            if (pos.x < rect.xMin)
                rect.xMin = pos.x;
            if (pos.x > rect.xMax)
                rect.xMax = pos.x;
            if (pos.y < rect.yMin)
                rect.yMin = pos.y;
            if (pos.y > rect.yMax)
                rect.yMax = pos.y;
        }
    }
    public void CreateBackgrounds(Transform parent,Element[] elements)
    {
        GameObject pobj = new GameObject("Backgrounds");
        pobj.transform.SetParent(parent);
        if (this.defaultBackground > -1)
        {
            Debug.Log($"DefaultBackground {this.defaultBackground}");
            var defaultBackground = elements[(int)this.defaultBackground].CreateInstance();
            defaultBackground.name = "defaultBackground";
            defaultBackground.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            Rect backgroundRect = new Rect(0,0,0,0);
            DetectionRect(backgrounds,ref backgroundRect);
            DetectionRect(sleepers,ref backgroundRect);
            DetectionRect(beds, ref backgroundRect);
            DetectionRect(decorates,ref  backgroundRect);
            defaultBackground.transform.position = backgroundRect.center;
            defaultBackground.GetComponent<SpriteRenderer>().size = backgroundRect.size;
        }
        for (int i = 0; i < backgrounds.Length; i++)
        {
            var background = backgrounds[i];
            var item = elements[background.materialId];
            var obj = item.CreateInstance();
            obj.name = $"background {i}";
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Background";
            obj.transform.position = GetPos(background);
            obj.transform.SetParent(pobj.transform);
        }
    }
    public BedBehaviour[] CreateBeds(Transform parent, Element[] elements)
    {
        List<BedBehaviour> bedsSprites = new List<BedBehaviour>();
        GameObject pobj = new GameObject("Beds");
        pobj.transform.SetParent(parent);
        for (int i = 0; i < beds.Length; i++)
        {
            var bed = beds[i];
            var item = elements[bed.materialId];
            var obj = item.CreateInstance();
            var bb = obj.AddComponent<BedBehaviour>();
            bb.SetData(elements[bed.materialId] as Bed);
            bedsSprites.Add(bb);
            obj.name = $"bed {i}";
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Other";
            obj.transform.position = GetPos(bed);
            obj.transform.SetParent(parent);
            obj.transform.SetParent(pobj.transform);
        }
        return bedsSprites.ToArray();
    }
    public void CreateDecorates(Transform parent, Element[] elements)
    {
        GameObject pobj = new GameObject("Decorates");
        pobj.transform.SetParent(parent);
        for (int i = 0; i < decorates.Length; i++)
        {
            var decorate = decorates[i];
            var item = elements[decorate.materialId];
            var obj = item.CreateInstance();
            obj.name = $"decorates {i}";
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Other";
            obj.transform.position = GetPos(decorate);
            obj.transform.SetParent(parent);
            obj.transform.SetParent(pobj.transform);
        }
    }
}
[System.Serializable]
public class SceneItem
{
    public int materialId;
    public float xPos;
    public float yPos;
}
