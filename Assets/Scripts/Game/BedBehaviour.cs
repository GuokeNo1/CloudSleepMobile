using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedBehaviour : MonoBehaviour
{
    [SerializeField]private Bed data;
    public void SetData(Bed data)
    {
        this.data = data;
    }

    public void SetType(int n)
    {
        if (n < 0)
        {
            GetComponent<SpriteRenderer>().sprite = data.mainSprite;
            return;
        }
        GetComponent<SpriteRenderer>().sprite = data.subSprites[n];
    }
    public bool isEmpty()
    {
        return GetComponent<SpriteRenderer>().sprite == data.mainSprite;
    }
    public Vector3 GetPos()
    {
        return transform.position + (Vector3)gameObject.GetComponent<BoxCollider2D>().offset;
    }
}
