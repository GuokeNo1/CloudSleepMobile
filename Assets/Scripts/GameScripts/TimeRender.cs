using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRender : MonoBehaviour
{
    [SerializeField] private Material material;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
