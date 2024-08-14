using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Bed : Element
{
    public string[] sleepfilenames;
    public float[] hitbox;
    public Sprite[] subSprites;
    public override void Load(DirectoryInfo dir)
    {
        if (isLoaded)
            return;
        base.Load(dir);
        subSprites = new Sprite[sleepfilenames.Length];
        for(int i = 0; i < sleepfilenames.Length; i++)
        {
            var filename = sleepfilenames[i];
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
        var collider = obj.AddComponent<BoxCollider2D>();
        Rect rect = new Rect();
        rect.xMin = hitbox[0];
        rect.yMin = mainSprite.texture.height - hitbox[3];
        rect.xMax = hitbox[2];
        rect.yMax = mainSprite.texture.height - hitbox[1];
        collider.size = rect.size * .01f;
        var pivot = new Vector2(offset[0], mainSprite.texture.height - offset[1]);
        collider.offset = (rect.center - pivot) * .01f;
        return obj;
    }
}