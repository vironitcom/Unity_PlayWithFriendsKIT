/***************************** Module Header *****************************\
Module Name:  ChatManager.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

This is main class for game with friends.
After we login with facebook, we will get user ID and Friends ID list.
Do connect with Photon Chat and set user with inline status.
You can change user status when he start play game or anything else 

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;
using Facebook.Unity;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    /// <summary>
    /// It will be singleton and will eble in all scenes
    /// </summary>
    public static ChatManager instance;
    /// <summary>
    /// Current user
    /// </summary>
    private ChatClient chatClient;
    /// <summary>
    /// Need to set here your ChatAppID;
    /// </summary>
    [SerializeField] private string ChatSettingsAppId;
    /// <summary>
    /// Can connect to a few channels. Default connect to global;
    /// </summary>
    [SerializeField] private string[] ChannelsToJoinOnConnect = new string[] { "global" };
    /// <summary>
    /// Current player Facebook ID
    /// </summary>
    private string UserID;
    /// <summary>
    /// Current player's friends Facebook ID
    /// </summary>
    private List<string> UserFriendsID;
    /// <summary>
    /// Friends dictionary (FriendsID, FrinedsName) 
    /// </summary>
    private Dictionary<string, string> UserFriendsName;
    /// <summary>
    /// Is current user connect to Photon Chat
    /// </summary>
    public bool IsChatConnect { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ActionManager.Action_OnPrivateMessageSend += SendPrivateMessage;
        ActionManager.Action_OnFacebookLogin += GetInfoForChat;
    }

    private void OnDestroy()
    {
        ActionManager.Action_OnPrivateMessageSend -= SendPrivateMessage;
        ActionManager.Action_OnFacebookLogin -= GetInfoForChat;
    }

    public void Update()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Service(); 
        }
    }

    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect(); ///!!! or will Unity Editor crash
        }
    }

    private void SendPrivateMessage(string friendID, string message)
    {
        chatClient.SendPrivateMessage(friendID, message);
    }
    /// <summary>
    /// Will be called after facebook login
    /// </summary>
    /// <param name="result"></param>
    private void GetInfoForChat(IResult result) 
    {
        UserID = result.ResultDictionary["id"].ToString();
        var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
        var friendsDict = (Dictionary<string, object>)dictionary["friends"];
        var friendsList = (List<object>)friendsDict["data"];
        UserFriendsID = new List<string>();
        UserFriendsName = new Dictionary<string, string>();
        foreach (var dict in friendsList)
        {
            UserFriendsID.Add(((Dictionary<string, object>)dict)["id"].ToString());
            UserFriendsName.Add(((Dictionary<string, object>)dict)["id"].ToString(), ((Dictionary<string, object>)dict)["name"].ToString());
        }
        ///Start chat
        Connect();
    }

    public void Connect()
    {
        this.chatClient = new ChatClient(this);
        this.chatClient.UseBackgroundWorkerForSending = true;
        this.chatClient.Connect(ChatSettingsAppId, "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues(UserID));

        IsChatConnect = true;
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.WARNING)
            Debug.LogWarning(message);
        else if (level == DebugLevel.ERROR)
            Debug.LogError(message);
        else
            Debug.Log(message);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange() Player state: " + state);
    }

    public void OnConnected()
    {
        Debug.Log("OnConnected() chat");
        if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
        {
            this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, 0);
        }
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
        chatClient.AddFriends(UserFriendsID.ToArray());

        ActionManager.Action_OnChatConnect(UserFriendsID, UserFriendsName, UserID);
    }

    public void OnDisconnected()
    {
        Debug.Log("OnDisconnected() chat");
        IsChatConnect = false;
        ActionManager.Action_OnChatDisconnect();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log("OnGetMessages()");
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage() " + sender + ": " + message);
        ActionManager.Action_OnGetPrivateMessage(sender, message);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("OnStatusUpdate() " + user + " status: " + status);
        ActionManager.Action_OnUsersStatusUpdate(user, status);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("OnSubscribed()");
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("OnUnsubscribed()");
    }
}

