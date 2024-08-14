using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using UnityEngine.Networking;

public class PackageCompression
{
    public static void ExtrPackages()
    {
        var data = Resources.Load<TextAsset>("packages").bytes;
        var stream = new MemoryStream(data);
        ZipArchive zip = new ZipArchive(stream);
        if(!Directory.Exists($"{Application.persistentDataPath}/packages/"))
            Directory.CreateDirectory($"{Application.persistentDataPath}/packages/");
        zip.ExtractToDirectory($"{Application.persistentDataPath}/packages/");
        zip.Dispose();
        stream.Close();
        stream.Dispose();
    }
}
