using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private RawImage picturePreview;
    
    public void takePicture() {
        StartCoroutine(_takePicture());
    }
    private IEnumerator _takePicture()
    {
        var temp = picturePreview.texture;
        picturePreview.texture = null;
        yield return new WaitForSeconds(.1f);
        picturePreview.texture = temp;
        if(Application.platform == RuntimePlatform.Android)
        {
            string filename = "sdcard/DCIM/CloudSleep";
            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }
            filename += $"/{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}.jpg";
            var rt = m_Camera.activeTexture;
            Texture2D texture = new Texture2D(rt.width, rt.height);
            var temprt = RenderTexture.active;
            RenderTexture.active = rt;
            texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texture.Apply();
            RenderTexture.active = temprt;
            File.WriteAllBytes(filename, texture.EncodeToJPG());
            Destroy(texture);
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ToastManager.instance.SetToast("IOS暂时不支持存储照片");
        }
    }
    private void Update()
    {
        m_Camera.orthographicSize = Camera.main.orthographicSize * .5f;
    }
    private void OnEnable()
    {
        m_Camera.gameObject.SetActive(true);
        m_Camera.targetTexture = new RenderTexture(1080, 1920, 0);
        picturePreview.texture = m_Camera.targetTexture;
    }
    private void OnDisable()
    {
        m_Camera.gameObject.SetActive(false);
        m_Camera.targetTexture.Release();
    }
}
