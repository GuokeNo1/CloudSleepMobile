using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Decorate : Element
{
    public float[] hitbox;
    public override void Load(DirectoryInfo dir)
    {
        if (isLoaded)
            return;
        base.Load(dir);
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