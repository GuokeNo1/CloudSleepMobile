using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Sleeper : Element
{
    public string[] emotefilenames;
    public Sprite[] subSprites;
    public override void Load(DirectoryInfo dir)
    {
        if (isLoaded)
            return;
        base.Load(dir);
        subSprites = new Sprite[emotefilenames.Length];
        for (int i = 0; i < emotefilenames.Length; i++)
        {
            var filename = emotefilenames[i];
            var mainFile = dir.GetFiles(filename, SearchOption.AllDirectories);
            if (mainFile.Length > 0)
            {
                var mainTex = new Texture2D(1, 1);
                mainTex.LoadImage(File.ReadAllBytes(mainFile[0].FullName));
                mainTex.Apply();
                mainTex.filterMode = FilterMode.Point;
                var pivot = Vector2.one * .5f;
                if (offset != null && offset.Length == 2)
                {
                    pivot.x = offset[0] / mainTex.width;
                    pivot.y = 1 - offset[1] / mainTex.height;
                }
                subSprites[i] = Sprite.Create(mainTex, new Rect(0, 0, mainTex.width, mainTex.height), pivot);
                subSprites[i].name = filename;
            }
        }
        isLoaded = true;
    }
    public override GameObject CreateInstance()
    {
        var obj = base.CreateInstance();
        var sr = obj.GetComponent<SpriteRenderer>();
        sr.spriteSortPoint = SpriteSortPoint.Pivot;
        sr.sortingLayerName = "Other";
        return obj;
    }
}