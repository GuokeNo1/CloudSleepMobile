using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CameraRenderTexture : MonoBehaviour
{
    public static bool finash = false;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void init()
    {
        Task.Run(() =>
        {
            while (true)
            {
                SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
                if (finash)
                {
                    break;
                }
            }
        });
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void FINASH()
    {
        finash = true;
    }
}
