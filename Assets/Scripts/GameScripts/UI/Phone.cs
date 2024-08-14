using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField]
    private GameObject[] uis;
    public void closeAll()
    {
        foreach(var u in uis)
        {
            u.SetActive(false);
        }
    }
    [ContextMenu("¹Ø±ÕÊÖ»ú")]
    public void closePhone()
    {
        LeanTween.moveLocalX(gameObject, 300, 1).setEase(LeanTweenType.easeOutElastic);
        gameObject.LeanScale(Vector3.zero, 1).setEase(LeanTweenType.easeOutElastic);
    }
    public void open(int ui)
    {
        closeAll();
        transform.localScale = Vector3.zero;
        LeanTween.moveLocal(gameObject, Vector3.zero, 1).setEase(LeanTweenType.easeOutElastic);
        gameObject.LeanScale(Vector3.one, 1).setEase(LeanTweenType.easeOutElastic);
        uis[ui].SetActive(true);
    }
}
