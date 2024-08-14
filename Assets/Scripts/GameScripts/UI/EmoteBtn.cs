using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteBtn : MonoBehaviour
{
    [SerializeField] private NetWorkManager netWorkManager;
    public int index = 0;
    public void Click()
    {
        netWorkManager.Send("emote", $"{index}");
    }
}
