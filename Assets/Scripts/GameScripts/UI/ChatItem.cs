using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private Text text;
    [SerializeField] private GameObject newObj;
    [SerializeField] private ChatManager chatManager;
    private ChatList chatList;
    public void OnPointerClick(PointerEventData eventData)
    {
        chatManager.SetShowPrivate(chatList.id);
    }

    public void SetChatInfo(ChatList chatList)
    {
        this.chatList = chatList;
        UpdateInfo();
        chatList.updates.AddListener(UpdateInfo);
    }
    private void OnDestroy()
    {
        chatList.updates.RemoveListener(UpdateInfo);
    }
    private void UpdateInfo()
    {
        newObj.SetActive(chatList.isNew);
        text.text = chatList.tag;
    }
}
