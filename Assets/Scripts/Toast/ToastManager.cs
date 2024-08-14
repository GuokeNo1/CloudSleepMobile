using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public static ToastManager instance { get; private set; }
    [SerializeField] private GameObject prefab;
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetToast(string msg)
    {
        var toast = Instantiate(prefab, transform);
        toast.GetComponent<Toast>().SetMsg(msg);
    }
}
