using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContextualMenuManager : MonoBehaviour
{
    public static ContextualMenuManager Instance { get; private set; }
    private void Awake() 
    {
        if (Instance != null && Instance != this) { Destroy(this); } 
        else { Instance = this;} 
    }

    [HideInInspector]
    public GameObject contextualMenuOpened;

    public Search_ContextualMenu search_contextualmenu;
    public App_ContextualMenu app_contextualmenu;
    public User_ContextualMenu user_contextualmenu;
    public Group_ContextualMenu group_ContextualMenu;
    public Token_ContextualMenu token_ContextualMenu;
    
    public void OpenSearch_ContextualMenu(GameObject pos)
    {
        search_contextualmenu.transform.position = pos.transform.position;
        OpenContextualMenu(search_contextualmenu.gameObject);
    }
    public void OpenApp_ContextualMenu(GameObject pos, int idApp)
    {
        app_contextualmenu.transform.position = pos.transform.position;
        app_contextualmenu.idApp = idApp;
        OpenContextualMenu(app_contextualmenu.gameObject);
    }
    public void OpenUser_ContextualMenu(GameObject pos, string principalID, string username)
    {
        user_contextualmenu.transform.position = pos.transform.position;
        user_contextualmenu.principalID = principalID;
        user_contextualmenu.username = username;
        OpenContextualMenu(user_contextualmenu.gameObject);
    }
    public void OpenGroup_ContextualMenu(GameObject pos, int groupID, string groupName)
    {
        group_ContextualMenu.transform.position = pos.transform.position;
        group_ContextualMenu.groupID = groupID;
        group_ContextualMenu.groupName = groupName;
        OpenContextualMenu(group_ContextualMenu.gameObject);
    }
    public void OpenToken_ContextualMenu(GameObject pos, int tokenID, string tokenName)
    {
        token_ContextualMenu.transform.position = pos.transform.position;
        token_ContextualMenu.tokenID = tokenID;
        token_ContextualMenu.tokenName = tokenName;
        OpenContextualMenu(group_ContextualMenu.gameObject);
    }
    
    
    //OpenBase
    public void OpenContextualMenu(GameObject go_ContextualMenu)
    {
        if(contextualMenuOpened){ contextualMenuOpened.SetActive(false);}
        contextualMenuOpened = go_ContextualMenu;
        contextualMenuOpened.SetActive(true);
        EventSystem.current.SetSelectedGameObject(contextualMenuOpened);
    }
    public void CloseContextualMenu()
    {
        if(contextualMenuOpened){ contextualMenuOpened.SetActive(false);}
       // EventSystem.current.SetSelectedGameObject(null);
    }
    



    
}
