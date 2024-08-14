using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NamePannel : MonoBehaviour
{
    [SerializeField] private Text Name;
    [SerializeField] private GameObject chartPrefab;
    public string NickName { get=>Name.text;}
    private void Awake()
    {
        foreach (ContentSizeFitter child in GetComponentsInChildren<ContentSizeFitter>(true))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
        }
    }
    public void SetName(string name)
    {
        Name.text = name;
        foreach (ContentSizeFitter child in GetComponentsInChildren<ContentSizeFitter>(true))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
        }
    }
    public void Chart(string content)
    {
        var chart = Instantiate(chartPrefab,transform.GetChild(0));
        chart.GetComponentInChildren<Text>().text = content;
    }
}
