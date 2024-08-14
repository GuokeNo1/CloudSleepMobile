using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public ChatList publicChat = new ChatList();
    public Dictionary<int,ChatList> historys = new Dictionary<int,ChatList>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parnet;
    [SerializeField] private ToggleGroup toggles;
    [SerializeField] private Toggle publicToggle;
    [SerializeField] private GameObject privateParnet;
    [SerializeField] private GameObject privateItemPrefab;
    [SerializeField] private Transform privateContent;
    bool isPublicMode = true;
    int currentid = 0;
    private void Awake()
    {
        publicToggle.onValueChanged.AddListener(pubmode);
    }
    private void pubmode(bool ispub)
    {
        currentid = -1;
        isPublicMode = ispub;
        privateParnet.SetActive(!ispub);
        if (!ispub)
            ReloadPrivate();
        else
            ShowHistory();
    }
    public void UpdatePrivate()
    {
        if (!isPublicMode&&currentid==-1)
        {
            ReloadPrivate();
        }
    }
    private void ReloadPrivate()
    {
        List<GameObject> destorys = new List<GameObject>();
        for (int i = 0; i < privateContent.childCount; i++)
        {
            var temp = privateContent.GetChild(i).gameObject;
            if (temp.activeSelf)
                destorys.Add(temp);
        }
        for (int i = 0; i < destorys.Count; i++)
        {
            Destroy(destorys[i]);
        }
        var sleepers = SleeperManager.instance.sleepers;
        foreach(var sleeper in sleepers)
        {
            var id = int.Parse(sleeper.Key);
            if (!historys.ContainsKey(id))
            {
                historys.Add(id, new ChatList()
                {
                    id = id,
                    isNew = false,
                    tag = SleeperManager.instance.GetInfoName(sleeper.Key)
                });
            }
        }
        foreach(var n in historys)
        {
            if (!sleepers.ContainsKey(n.Key.ToString())&&!n.Value.tag.StartsWith("(已离开)"))
            {
                n.Value.tag = $"(已离开){n.Value.tag}";
            }
            var obj = Instantiate(privateItemPrefab, privateContent);
            var item = obj.GetComponent<ChatItem>();
            item.SetChatInfo(n.Value);
            obj.SetActive(true);
        }
    }
    public void SetShowPrivate(int id)
    {
        currentid = id;
        privateParnet.SetActive(false);
        ShowHistory();
    }
    public void SetChat(string msg,string id,bool isPublic = true)
    {
        if (isPublic)
        {
            publicChat.chatHistory.Add(msg);
            if (isPublicMode)
            {
                AddChart(msg);
            }
        }
        else
        {
            int iid = int.Parse(id);
            if (!historys.ContainsKey(iid))
            {
                historys.Add(iid, new ChatList() { tag = SleeperManager.instance.GetInfoName(id), id = iid });
            }
            historys[iid].chatHistory.Add(msg);
            if (currentid == iid)
            {
                AddChart(msg);
            }
            else
            {
                historys[iid].isNew = true;
            }
            historys[iid].Update();
            if (currentid == -1)
            {
                ReloadPrivate();
            }
        }
    }
    public void Send(string msg)
    {
        msg = msg.Trim();
        if (msg == "")
            return;
        if (isPublicMode)
        {
            SleeperManager.instance.SendChart(msg);
        }
        else
        {
            //SetChat($"[我]:{msg}", currentid.ToString(), false);
            SleeperManager.instance.SendPriChat(currentid.ToString(), msg);
        }
    }
    public void Pub()
    {
        isPublicMode = true;
        ShowHistory();
    }
    public void Priv()
    {
        isPublicMode = false;
        //ShowHistory();
    }
    public void ShowHistory()
    {
        List<GameObject> destorys = new List<GameObject>();
        for(int i = 0; i < parnet.childCount; i++)
        {
            var temp = parnet.GetChild(i).gameObject;
            if(temp.activeSelf)
                destorys.Add(temp);
        }
        for(int i = 0; i < destorys.Count; i++)
        {
            Destroy(destorys[i]);
        }
        destorys.Clear();
        if (isPublicMode)
        {
            for(int i = 0; i < publicChat.chatHistory.Count; i++)
            {
                var msg = publicChat.chatHistory[i];
                AddChart(msg);
            }
        }
        else
        {
            for(int i=0;i<historys[currentid].chatHistory.Count; i++)
            {
                var msg = historys[currentid].chatHistory[i];
                historys[currentid].isNew = false;
                AddChart(msg);
            }
        }
    }
    private void AddChart(string msg)
    {
        var obj = Instantiate(prefab, parnet);
        obj.GetComponent<Text>().text = msg;
        obj.SetActive(true);
    }
}
public class ChatList
{
    public int id;
    public string tag;
    public List<string> chatHistory = new List<string>();
    public bool isNew = false;
    public UnityEvent updates = new UnityEvent();
    public void Update()
    {
        updates?.Invoke();
    }
}