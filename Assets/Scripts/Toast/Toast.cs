using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private float HP;
    [SerializeField] private float VP;
    public void SetMsg(string msg)
    {
        var rect = GetComponent<RectTransform>();
        text.text = msg;
        var width = text.preferredWidth;
        if (width + 2 * HP > 1080)
        {
            width = 1080;
        }
        else
        {
            width = width + 2 * HP;
        }
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        var height = text.preferredHeight + 2 * VP;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.LeanAlpha(1, .5f).setOnComplete(() =>
        {
            cg.LeanAlpha(0, .5f).setDelay(10).setOnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
}
