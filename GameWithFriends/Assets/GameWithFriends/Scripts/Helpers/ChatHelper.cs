/***************************** Module Header *****************************\
Module Name:  ChatHelper.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

This script made for helping ChatManager.cs
Will be able in MenuScene 

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\**************************************************************************/
using System.Collections.Generic;
using UnityEngine;

public class ChatHelper : MonoBehaviour
{
    /// <summary>
    ///  Prefab for create friends list
    /// </summary>
    [SerializeField] private GameObject FriendListItemPrefab;
    /// <summary>
    /// Tranform where friends list have to be create
    /// </summary>
    [SerializeField] private Transform FriendListItemParent;
    /// <summary>
    /// Popup that will be called on user's friend side
    /// </summary>
    [SerializeField] private GameObject InvatePanelPopup;
    /// <summary>
    /// Popup that will get current user after invate friend to play
    /// </summary>
    [SerializeField] private GameObject WaitPanelPopup;
    /// <summary>
    /// Screen that came when game will created
    /// </summary>
    [SerializeField] private GameObject WaitingForStartMatchPanel;

    private readonly Dictionary<string, FriendItemHelper> friendListItemDictionary = new Dictionary<string, FriendItemHelper>();
    

    /// <summary>
    /// base chat messagees
    /// </summary>
    private const string INVATE = "invate";
    private const string OK = "ok";
    private const string NO = "no";
    private const string CANCEL = "cancel";

    /// <summary>
    /// Current player Facebook ID
    /// </summary>
    private string UserID;
    /// <summary>
    /// ID of player that current player send invate to
    /// </summary>
    public string SelectedFriendID { get; private set; }
    /// <summary>
    /// ID for users friend that what to invate current player to play with
    /// </summary>
    public string InvatedFriendID { get; private set; }
    /// <summary>
    /// Dictionaru of all current player's friends (ID, Name)
    /// </summary>
    public Dictionary<string, string> FriendsName { get; private set; }

    private void Start()
    {
        ActionManager.Action_OnChatConnect += CreateFriendsItem;
        ActionManager.Action_OnGetPrivateMessage += GetPrivateMessage;
        ActionManager.Action_OnUsersStatusUpdate += PlayerStatusUpdate;
        ActionManager.Action_OnChatDisconnect += DestroyFrindsItem;
        ActionManager.Action_OnInvateFriend += SelectFriend;

    }

    private void OnDestroy()
    {
        ActionManager.Action_OnChatConnect -= CreateFriendsItem;
        ActionManager.Action_OnGetPrivateMessage -= GetPrivateMessage;
        ActionManager.Action_OnUsersStatusUpdate -= PlayerStatusUpdate;
        ActionManager.Action_OnChatDisconnect -= DestroyFrindsItem;
        ActionManager.Action_OnInvateFriend -= SelectFriend;
    }

    private void DestroyFrindsItem()
    {
        foreach (var item in FindObjectsOfType<FriendItemHelper>())
            Destroy(item.gameObject);
    }

    private void CreateFriendsItem(List<string> FriendsList, Dictionary<string, string> FriendsName, string UserID)
    {
        this.UserID = UserID;
        this.FriendsName = FriendsName;
        if (FriendsList != null && FriendsList.Count > 0)
            foreach (string _friend in FriendsList)
                this.InstantiateFriendButton(_friend);
    }

    private void InstantiateFriendButton(string friendId)
    {
        GameObject friendItemm = Instantiate(this.FriendListItemPrefab, FriendListItemParent);
        FriendItemHelper _friendItem = friendItemm.GetComponent<FriendItemHelper>();
        _friendItem.FriendId = friendId;
        _friendItem.CreateFriend(friendId);
        this.friendListItemDictionary[friendId] = _friendItem;
    }

    public void SelectFriend(string id)
    {
        SelectedFriendID = id;
    }

    private void GetPrivateMessage(string sender, object message)
    {
        ///current player send "invate" to friend
        if (message.ToString() == INVATE)
        {
            if (sender != UserID)
            {
                InvatedFriendID = sender;
                InvatePanelPopup.SetActive(true);
            }
            else
            {
                WaitPanelPopup.SetActive(true);
            }
        }
        ///Answer from current user friend
        if (message.ToString() == NO)
        {
            if (sender != UserID)
            {
                SelectedFriendID = string.Empty;
                WaitPanelPopup.SetActive(false);
            }
            else
            {
                InvatedFriendID = string.Empty;
                InvatePanelPopup.SetActive(false);
            }
        }
        ///Current user cancel invate
        if (message.ToString() == CANCEL)
        {
            if (sender != UserID)
            {
                InvatedFriendID = string.Empty;
                InvatePanelPopup.SetActive(false);
            }
            else
            {
                SelectedFriendID = string.Empty;
                WaitPanelPopup.SetActive(false);
            }
        }
        ///Friend of currend user ready to play
        if (message.ToString() == OK)
        {
            if (sender != UserID)
            {
                WaitPanelPopup.SetActive(false);
                ActionManager.Action_DoCreateFriendRoom(SelectedFriendID);
            }
            else
            {
                InvatePanelPopup.SetActive(false);
                ActionManager.Action_DoJoinFriendRoom(InvatedFriendID);
            }
        }
    }

    private void PlayerStatusUpdate(string user, int status)
    {
        if (friendListItemDictionary.ContainsKey(user))
        {
            FriendItemHelper _friendItem = friendListItemDictionary[user];
            if (_friendItem != null)
                _friendItem.OnFriendStatusUpdate(status);
        }
    }
}