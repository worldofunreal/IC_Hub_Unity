using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationsPanelManager : MonoBehaviour
{
    public static NotificationsPanelManager Instance { get; private set; }
    private void Awake() 
    {
        if (Instance != null && Instance != this) { Destroy(this); } 
        else { Instance = this;} 
    }
    
    public GameObject panelParent;
    public TMP_Text allNotificationsNumber;
    public TMP_Text allNotificationsNumberChatClosed;
    
    [Header("Request Friends Panel : ")]
    public GameObject prefabFriendRequest;
    public GameObject contentFriendRequests;
    public TMP_Text requestFriendNumber;

    [Header("Request Groups Panel : ")]
    public GameObject prefabGroupRequest;
    public GameObject contentGroupRequests;
    public TMP_Text requestGroupNumber;
    
    [DllImport("__Internal")]
    private static extern void JSAcceptFriendRequest(string principalID);
    [DllImport("__Internal")]
    private static extern void JSDenyFriendRequest(string principalID);
    
    [System.Serializable]
    public class Notification {
        public string title;
        public string avatar;
        public string description;
    }
    [System.Serializable]
    public class RequestsFriend {
        public string username;
        public string principalID;
        public string avatarUser;
    }
    [System.Serializable]
    public class InfoNotificationPanel { 
        public List<Notification> notifications;
        public List<RequestsFriend> requests;
    }
    
    public void ClosePopup() { panelParent.SetActive(false); }
    public void OpenPopup() { panelParent.SetActive(true); }

    private void Start()
    {
        //GetInfoNotificationPanel(stringJson);
    }

    public void GetInfoNotificationPanel(string json)
    {
        InfoNotificationPanel infoNotificationPanel = JsonUtility.FromJson<InfoNotificationPanel>(json);
        
    /*//Fill notification
        foreach (Transform t in contentNotification.transform) { GameObject.Destroy(t.gameObject); }
        foreach (Notification g in infoNotificationPanel.notifications)
        {
           
        }*/
    //Fill request
    Pool_PrefabsGO.Instance.Release_AllObjsInPool(Pool_PrefabsGO.Instance.poolNotificationRequests);
        
        foreach (RequestsFriend g in infoNotificationPanel.requests)
        {
            AddRequestToList(g.principalID, g.username, g.avatarUser);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentFriendRequests.GetComponent<RectTransform>());
        requestFriendNumber.text = infoNotificationPanel.requests.Count.ToString();

        allNotificationsNumber.text = infoNotificationPanel.requests.Count.ToString();
        allNotificationsNumberChatClosed.text = infoNotificationPanel.requests.Count.ToString();
    }
    
    public void AddRequestToList(string principalID, string username, string avatarUser){
        GameObject newRequest = Pool_PrefabsGO.Instance.Get_ObjFromPool(Pool_PrefabsGO.Instance.poolNotificationRequests);
        RequestFriendPrefab requestFriendPrefab = newRequest.GetComponent<RequestFriendPrefab>();

        requestFriendPrefab.userNameText.text = username;
        requestFriendPrefab.principalID.text = principalID.Substring(0, 4)+"..."+principalID.Substring(principalID.Length - 4);
        requestFriendPrefab.icon.ChangeUrlImage(avatarUser);
        requestFriendPrefab.buttonAccept.onClick.RemoveAllListeners(); requestFriendPrefab.buttonDeny.onClick.RemoveAllListeners();
        
        requestFriendPrefab.buttonAccept.onClick.AddListener(() =>
        {
            CanvasPopup.Instance.OpenPopup(() =>
            {
                CanvasPopup.Instance.OpenLoadingPanel();
                JSAcceptFriendRequest(principalID);
            }, null, "Accept", "Cancel", "Do you want accept this User?", username, principalID, avatarUser);
        });
        requestFriendPrefab.buttonDeny.onClick.AddListener(() =>
        {
            CanvasPopup.Instance.OpenPopup(() =>
            {
                CanvasPopup.Instance.OpenLoadingPanel();
                JSDenyFriendRequest(principalID);
            }, null, "Deny", "Cancel", "Do you want deny this User?", username, principalID, avatarUser);
        });
    }
}
