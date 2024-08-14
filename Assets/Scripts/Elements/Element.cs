using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Element
{
    public string filename;
    public float[] offset;
    public Sprite mainSprite;
    protected bool isLoaded = false;

    public virtual void Load(DirectoryInfo dir)
    {
        if (isLoaded)
            return;
        var mainFile = dir.GetFiles(filename, SearchOption.AllDirectories);
        if (mainFile.Length > 0)
        {
            var  mainTex = new Texture2D(1,1);
            mainTex.LoadImage(File.ReadAllBytes(mainFile[0].FullName));
            mainTex.Apply();
            mainTex.filterMode = FilterMode.Point;
            var pivot = Vector2.one*.5f;
            if (offset != null &&  offset.Length == 2) {
                pivot.x = offset[0] / mainTex.width;
                pivot.y = 1 - offset[1] / mainTex.height;
            }
            mainSprite = Sprite.Create(mainTex, new Rect(0, 0, mainTex.width, mainTex.height), pivot);
            mainSprite.name = filename;
        }
        if (this.GetType() == typeof(Element))
            isLoaded = true;
    }
    public virtual GameObject CreateInstance()
    {
        GameObject obj = new GameObject();
        obj.name = filename;
        obj.transform.position = Vector3.zero;
        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = mainSprite;
        sr.spriteSortPoint = SpriteSortPoint.Pivot;
        return obj;
    }
}
[System.Serializable]
public class ElementList<T>
{
    public T[] materials;
    public void Load(DirectoryInfo dir)
    {
        for(int i = 0; i < materials.Length; i++)
        {
            var material = materials[i] as Element;
            material.Load(dir);
        }
    }
}