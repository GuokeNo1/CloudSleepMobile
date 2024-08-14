using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRenderController : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private int hour;
    [SerializeField] private int minute;
    [SerializeField] private int second;
    [SerializeField] private double sconds;
    private void Update()
    {
        DateTime dt = DateTime.Now;
        var time = new TimeSpan(dt.Hour,dt.Minute,dt.Second);
        float n = 1;
        if(time.TotalSeconds < 6 * 60 * 60)
        {
            n = (float)(1-time.TotalSeconds / (6 * 60 * 60));
        }else if(time.TotalSeconds >= 18 * 60 * 60)
        {
            n = (float)(time.TotalSeconds-18 * 60 * 60) / ((24 - 18) * 60 * 60);
        }
        else
        {
            n = 0;
        }
        material.SetFloat("_Range", n);
    }
}
