using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleeperManager : MonoBehaviour
{
    public static SleeperManager instance { get; private set; }
    private Dictionary<string, SleeperBehaviour> datas = new Dictionary<string, SleeperBehaviour>();
    private Sleeper[] sleeperTypes;
    [SerializeField] private BedManager bedManager;
    [SerializeField] private NetWorkManager netWorkManager;
    [SerializeField] private PlayerInput inputManager;
    [SerializeField] private Transform emoteParnet;
    [SerializeField] private GameObject emoteObj;
    [SerializeField] private ChatManager chatManager;
    private GameObject[] emoteBtns;
    public Dictionary<string, SleeperBehaviour> sleepers { get { return datas; } }
    private string MyId;
    private bool allwaysLookMe = false;
    private void Awake()
    {
        instance = this;
    }
    public string GetInfoName(string id)
    {
        return $"{id}@{datas[id].UserName}";
    }
    public void SetTypes(Sleeper[] data)
    {
        sleeperTypes = data;
    }
    private void CreateSleeper(string id)
    {
        var obj = sleeperTypes[0].CreateInstance();
        var sb = Instantiate(Resources.Load<GameObject>("SleeperPrefab")).GetComponent<SleeperBehaviour>();
        sb.body = obj;
        obj.transform.SetParent(sb.transform, false);
        sb.gameObject.name = $"Sleeper@{id}";
        var ani = obj.AddComponent<Animator>();
        sb.SetAnimator(ani);
        obj.transform.localPosition = Vector3.zero;
        sb.SetData(sleeperTypes[0]);
        sb.SetME();
        datas.Add(id, sb);
    }
    public void Leave(string id)
    {
        if (datas.ContainsKey(id))
        {
            var data = datas[id];
            AddToChart($"<color=\"#00F\">睡客[{id}@{data.UserName}]走咯~</color>");
            datas.Remove(id);
            data.Leave();
            chatManager.UpdatePrivate();
        }
    }
    public void Sleeper(string id, params string[] args)
    {
        if (!datas.ContainsKey(id)) {
            CreateSleeper(id);
            AddToChart($"<color=\"#00F\">新睡客{id}来咯~</color>");
        }
    }
    public void Move(string id, params string[] args)
    {
        if (datas.ContainsKey(id))
        {
            datas[id].MoveTo(new Vector2(int.Parse(args[0]), -int.Parse(args[1])) * .01f);
        }
    }
    public void MoveTo(Vector2 pos)
    {
        netWorkManager.Send("move", ((int)(pos.x * 100)).ToString(), ((int)(-pos.y * 100)).ToString());
    }
    public void Pos(string id, params string[] args)
    {
        if (datas.ContainsKey(id))
        {
            datas[id].SetPos(new Vector2(int.Parse(args[0]), -int.Parse(args[1])) * .01f);
        }
    }
    public void AllwaysLookMeON()
    {
        allwaysLookMe = true;
    }
    public void AllwaysLookMeOFF()
    {
        allwaysLookMe = false;
    }
    public void LookAtMe()
    {
        if (datas.ContainsKey(MyId))
        {
            Vector2 pos = datas[MyId].gameObject.transform.position;
            var target = inputManager.DetechRect(pos);
            Camera.main.transform.position = target;
        }
    }
    public void SetPos(Vector2 pos)
    {
        pos = pos * 100;
        netWorkManager.Send("pos", ((int)pos.x).ToString(), ((int)-pos.y).ToString());
    }
    private void AddToChart(string msg)
    {
        //var obj = Instantiate(chartObj, chartParnet);
        //obj.GetComponent<Text>().text = msg;
        //obj.SetActive(true);
        //CancelInvoke("UpdateChart");
        //Invoke("UpdateChart", .5f);

        chatManager.SetChat(msg, "");
    }
    private void Update()
    {
        if (datas.ContainsKey(MyId)&&allwaysLookMe)
        {
            Vector2 pos = datas[MyId].gameObject.transform.position;
            var target = inputManager.DetechRect(pos);
            //Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, target, 10 * Time.deltaTime);
            Camera.main.transform.LeanMove(target, .5f);
        }
    }
    public void Yourid(string id)
    {
        if (!datas.ContainsKey(id))
        {
            CreateSleeper(id);
        }
        MyId = id;
        var pos = bedManager.MapRect.position + new Vector2(Random.Range(0, bedManager.MapRect.width), Random.Range(bedManager.MapRect.height, 0));
        datas[id].SetPos(pos);
        datas[id].SetME(true);
        pos = pos * 100;
        netWorkManager.Send("pos", ((int)pos.x).ToString(), ((int)-pos.y).ToString());
        LookAtMe();
    }
    public void SendChart(string msg)
    {
        msg = msg.Trim();
        if (msg == "")
            return;
        netWorkManager.Send("chat", msg);
    }
    public void SendGetUP()
    {
        netWorkManager.Send("getup", "0");
    }
    public void SendSleep(string id)
    {
        netWorkManager.Send("sleep", id);
    }
    public void Sleep(string id, params string[] args)
    {
        if (datas.ContainsKey(id))
        {
            datas[id].LastSleepId = int.Parse(args[0]);
            datas[id].Sleep(args[0]);
            bedManager.Sleep(args[0], datas[id].SleeperId);
        }
    }
    public void GetUp(string id,params string[] args)
    {
        if (datas.ContainsKey(id))
        {
            datas[id].getup();
            bedManager.GetUP(datas[id].LastSleepId.ToString());
        }
    }
    public void PriChat(string id, params string[] args)
    {
        var from = args[0];
        var to = args[1];
        var msg = args[2];
        if(to == MyId)
        {
            chatManager.SetChat($"[@{datas[from].UserName}]:{msg}", from, false);
        }
        else
        {
            chatManager.SetChat($"[@{datas[from].UserName}]:{msg}", to, false);
        }
    }
    public void SendPriChat(string to,string msg)
    {
        netWorkManager.Send("prichat", MyId, to, msg);
    }
    public void Type(string id, params string[] args)
    {
        if (datas.ContainsKey(id))
        {
            datas[id].SetData(sleeperTypes[int.Parse(args[0])]);
            datas[id].SleeperId = int.Parse(args[0]);
            if (id == MyId)
            {
                if (emoteBtns != null && emoteBtns.Length > 0)
                {
                    for(int i=0;i<emoteBtns.Length; i++)
                    {
                        Destroy(emoteBtns[i]);
                    }
                }
                var typedata = sleeperTypes[int.Parse(args[0])];
                emoteBtns = new GameObject[typedata.subSprites.Length];
                for(int i = 0; i < typedata.subSprites.Length; i++)
                {
                    emoteBtns[i] = Instantiate(emoteObj, emoteParnet);
                    var btn = emoteBtns[i].GetComponent<EmoteBtn>();
                    btn.index = i;
                    var img = emoteBtns[i].GetComponent<Image>();
                    img.sprite = typedata.subSprites[i];
                    emoteBtns[i].SetActive(true);
                }
            }
        }
    }
    public void Emote(int n) {
        netWorkManager.Send("emote", n.ToString());
    }
    public void Name(string id, params string[] args)
    {
        if (datas.ContainsKey(id))
        {
            datas[id].SetName(args[0]);
            AddToChart($"<color=\"#00F\">睡客{id}设定昵称为：{args[0]}</color>");
            chatManager.UpdatePrivate();
        }
    }
    public void Emote(string id, params string[] args)
    {
        if (datas.ContainsKey(id))
        {
            datas[id].SetEmote(int.Parse(args[0]));
        }
    }
    public void Chart(string id,params string[] args)
    {
        if (datas.ContainsKey(id))
        {
            datas[id].Chart(args[0]);
            AddToChart($"[@{datas[id].UserName}]:{args[0]}");
        }
    }
}
