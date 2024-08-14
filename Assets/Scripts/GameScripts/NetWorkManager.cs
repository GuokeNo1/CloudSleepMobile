using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetWorkManager : MonoBehaviour
{
    [SerializeField] private SleeperManager sleeperManager;
    Socket skt;
    public bool Connect()
    {
        string Ipendpoint = SceneDataManager.instance.ipport;
        string nickname = SceneDataManager.instance.nickname;
        string sleeper = SceneDataManager.instance.sleeperMid.ToString();
        var scene = SceneDataManager.instance.selectScene;
        Ipendpoint = Ipendpoint.Replace("：", ":").Replace("。", ".");
        if (!Ipendpoint.Contains(':'))
        {
            return false;
        }
        skt = new Socket(SocketType.Stream, ProtocolType.Tcp);
        string ip = Ipendpoint.Substring(0, Ipendpoint.IndexOf(':'));
        int port = int.Parse(Ipendpoint.Substring(Ipendpoint.IndexOf(':') + 1));
        try
        {
            var ipaddresses = Dns.GetHostAddresses(ip);
            bool isLogin = false;
            foreach(IPAddress ipaddress in ipaddresses)
            {
                try
                {
                    var IPENDPOINT = new IPEndPoint(ipaddress, port);
                    skt.Connect(IPENDPOINT);
                    //Heart();
                    Send(new NetWorkData()
                    {
                        Cmd = "packguid",
                        Args = new string[] { scene.info.guid },
                    });
                    Login(nickname, sleeper);
                    isLogin = true;
                    break;
                }
                catch { }
            }
            if (isLogin)
            {
                sleeperManager.SetTypes(scene.sleepers.materials);
            }
            return isLogin;
        }
        catch {
            return false;
        }
    }
    private void Send(NetWorkData data)
    {
        if(skt != null)
        {
            byte[] raw_data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data) + "\0");
            Debug.Log($"Send: {JsonUtility.ToJson(data)}");
            skt.Send(raw_data);
        }
    }
    public void Send(string cmd,params string[] args)
    {
        if (skt != null)
        {
            var data = new NetWorkData()
            {
                Cmd = cmd,
                Args = args,
                Id = null
            };
            Send(data);
        }
    }
    private void Heart()
    {
        if (skt != null)
        {
            byte[] raw_data = Encoding.UTF8.GetBytes("\0");
            skt.Send(raw_data);
        }
    }
    private void Login(string nickname,string sleeper) {
        Send(new NetWorkData()
        {
            Cmd = "name",
            Args = new string[] { nickname },
        });
        Send(new NetWorkData()
        {
            Cmd = "type",
            Args = new string[] { sleeper },
        });
    }
    private void Update()
    {
        ReceiveDetection();
    }
    private void ReceiveDetection()
    {
        if (skt == null || !skt.Connected)
        {
            SceneManager.LoadScene("MenuScene");
            ToastManager.instance.SetToast("连接断开!");
            return;
        }
        var available = skt.Available;
        if (available > 0)
        {
            byte[] buffer = new byte[available];
            var len = skt.Receive(buffer);
            var raw_data = Encoding.UTF8.GetString(buffer, 0, len);
            string[] datas = raw_data.Split('\0');
            foreach(var data in datas)
            {
                ProcessData(data);
            }
        }
    }
    private void ProcessData(string data)
    {
        if (data.Trim() == "")
            return;
        Debug.Log($"Receive: {data}");
        var jdata = JsonUtility.FromJson<NetWorkData>(data);
        switch (jdata.Cmd)
        {
            case "sleeper":
                sleeperManager.Sleeper(jdata.Id,jdata.Args);
                break;
            case "pos":
                sleeperManager.Pos(jdata.Id,jdata.Args);
                break;
            case "move":
                sleeperManager.Move(jdata.Id,jdata.Args);
                break;
            case "yourid":
                sleeperManager.Yourid(jdata.Id);
                break;
            case "name":
                sleeperManager.Name(jdata.Id, jdata.Args);
                break;
            case "type":
                sleeperManager.Type(jdata.Id, jdata.Args);
                break;
            case "emote":
                sleeperManager.Emote(jdata.Id, jdata.Args);
                break;
            case "chat":
                sleeperManager.Chart(jdata.Id, jdata.Args);
                break;
            case "leave":
                sleeperManager.Leave(jdata.Id);
                break;
            case "sleep":
                sleeperManager.Sleep(jdata.Id, jdata.Args);
                break;
            case "getup":
                sleeperManager.GetUp(jdata.Id, jdata.Args);
                break;
            case "prichat":
                sleeperManager.PriChat(jdata.Id, jdata.Args);
                break;
        }
    }
    private void OnDestroy()
    {
        if (skt != null&&skt.Connected)
        {
            skt.Disconnect(false);
            skt.Close();
            skt.Dispose();
        }
    }
    private void Awake()
    {
        if (Connect())
        {
            PlayerPrefs.SetString("nickname", SceneDataManager.instance.nickname);
            PlayerPrefs.Save();
        }
        else
        {
            SceneManager.LoadSceneAsync("MenuScene");
            ToastManager.instance?.SetToast("连接失败，无法与服务器建立连接");
        }
    }
}
[System.Serializable]
public class NetWorkData
{
    public string Cmd;
    public string[] Args;
    public string Id;
#if UNITY_EDITOR
    public override string ToString()
    {
        return $"{Cmd}-{string.Join(',',Args)}--{Id}";
    }
#endif
}
