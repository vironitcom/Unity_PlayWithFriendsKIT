/***************************** Module Header *****************************\
Module Name:  PhotonManager.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com


This main manager for network mutliplayer game
Here you can found a lot of debug information and callbacks


The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\**************************************************************************/
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonManager : Photon.MonoBehaviour, IPunCallbacks
{
    /// <summary>
    /// Singletone that will start with app and life for all time
    /// </summary>
    public static PhotonManager instance { get; private set; }

    [SerializeField] private byte Version = 1;
    [SerializeField] private string GameSceneName = "GameScene";
    [SerializeField] private string MenuScemeName = "MenuScene";
    /// <summary>
    /// Can be any count of player
    /// For testing set 1 player and game will start without waiting for opponents
    /// </summary>
    private byte CountPlayerInGame;

    public void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }
    public void Start()
    {
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.ConnectUsingSettings(Version.ToString());
        ActionManager.Action_DoLeaveRoom += DoLeaveRoom;
        ActionManager.Action_DoJoinRandomRoom += DoJoinRandomRoom;
        ActionManager.Action_DoCreateRoom += DoCreateRoom;
        ActionManager.Action_DoCreateFriendRoom += DoCreateFriendRoom;
        ActionManager.Action_DoJoinFriendRoom += DoJoinFriendRoom;
    }

    public void OnDestroy()
    {
        ActionManager.Action_DoLeaveRoom -= DoLeaveRoom;
        ActionManager.Action_DoJoinRandomRoom -= DoJoinRandomRoom;
        ActionManager.Action_DoCreateRoom -= DoCreateRoom;
        ActionManager.Action_DoCreateFriendRoom -= DoCreateFriendRoom;
        ActionManager.Action_DoJoinFriendRoom -= DoJoinFriendRoom;
    }

    #region GAME_WITH_FRIENDS
    /// <summary>
    /// Need to be login with Facebook
    /// </summary>
    /// <param name="friendID">
    /// User friends FacebookID
    /// </param>
    private void DoCreateFriendRoom(string friendID)
    {
        CountPlayerInGame = 2;
        PhotonNetwork.JoinOrCreateRoom(Facebook.Unity.AccessToken.CurrentAccessToken.UserId + friendID, new RoomOptions { MaxPlayers = CountPlayerInGame, PublishUserId = true }, null);
    }
    private void DoJoinFriendRoom(string friendID)
    {
        CountPlayerInGame = 2;
        PhotonNetwork.JoinOrCreateRoom(friendID + Facebook.Unity.AccessToken.CurrentAccessToken.UserId, new RoomOptions { MaxPlayers = CountPlayerInGame, PublishUserId = true }, null);
    }

    #endregion

    #region USERS_FUNCTIONS
    private void DoJoinRandomRoom(byte playersAmount)
    {
        CountPlayerInGame = playersAmount;
        Debug.Log("DoJoinRandomRoom() for " + CountPlayerInGame + " players");
        PhotonNetwork.JoinRandomRoom(null, CountPlayerInGame);
    }

    private void DoCreateRoom(byte playersAmount)
    {
        CountPlayerInGame = playersAmount;
        Debug.Log("DoCreateRoom() for " + CountPlayerInGame + " players");
        ///Here you can set any room name and other users will see name or price for room
        PhotonNetwork.CreateRoom("TestRoom" + UnityEngine.Random.Range(0, 1000), new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = CountPlayerInGame }, null);
    }

    private void DoLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region MAIN_CALLBACKS
    public virtual void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected " + newPlayer);

        if (PhotonNetwork.room.PlayerCount == CountPlayerInGame)
        {
            PhotonNetwork.LoadLevel(GameSceneName);
        }

        ActionManager.Action_OnPhotonPlayerConnected();
    }
    public virtual void OnJoinedRoom()
    {
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.playerName = "TestUser_" + UnityEngine.Random.Range(0, int.MaxValue);        
        if (CountPlayerInGame == 1)
        {
            PhotonNetwork.LoadLevel(GameSceneName);
        }
        ActionManager.Action_OnJoinedRoom();
    }


    public virtual void OnLeftRoom()
    {
        Debug.Log("OnLeaveRoom");
        ActionManager.Action_OnLeaveRoom();
    }

    public virtual void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonJoinRoomFailed()");
        foreach (var item in codeAndMsg)
        {
            Debug.Log(item.ToString());
        }
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = CountPlayerInGame }, null);
        Debug.Log("CreateRoom() for players: " + CountPlayerInGame);
    }

    public virtual void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonRandomJoinFailed");
        foreach (var item in codeAndMsg)
        {
            Debug.Log(item.ToString());
        }
        PhotonNetwork.CreateRoom("Test" + UnityEngine.Random.Range(0, 1000), new RoomOptions() { MaxPlayers = (byte)CountPlayerInGame }, null);
        Debug.Log("CreateRoomForRandomGame() for players: " + CountPlayerInGame);
    }

    public virtual void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.Log("OnMasterClientSwitched()");
        Debug.Log("New MasterClien is " + newMasterClient);
    }

    public virtual void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom()");
        ActionManager.Action_OnCreatedRoom();
    }

    public virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerDisconnected  " + otherPlayer);
    }

    public virtual void OnReceivedRoomListUpdate()
    {
        Debug.Log("OnReceivedRoomListUpdate()");
        ActionManager.Action_OnRoomListUpdate(PhotonNetwork.GetRoomList());
    }

    #endregion

    #region DEBUG_INFO

    public virtual void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster()");
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby()");
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.Log("OnFailedToConnectToPhoton() " + cause);
    }


    public virtual void OnConnectedToPhoton()
    {
        Debug.Log("OnConnectedToPhoton");
    }

    public virtual void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonCreateRoomFailed()");
        foreach (var item in codeAndMsg)
        {
            Debug.Log(item.ToString());
        }
    }

    public virtual void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby()");
    }

    public virtual void OnConnectionFail(DisconnectCause cause)
    {
        Debug.Log("OnConnectionFail: " + cause);
    }

    public virtual void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton()");
    }

    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonInstantiate " + info);
    }

    public virtual void OnPhotonMaxCccuReached()
    {
        Debug.Log("OnPhotonMaxCccuReached()");
    }

    public virtual void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("OnPhotonCustomRoomPropertiesChanged" + propertiesThatChanged);
    }

    public virtual void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        Debug.Log("OnPhotonPlayerPropertiesChanged");
        foreach (var item in playerAndUpdatedProps)
        {
            Debug.Log(item.ToString());
        }
    }

    public virtual void OnUpdatedFriendList()
    {
        Debug.Log("OnUpdatedFriendList()");
    }

    public virtual void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed() " + debugMessage);
    }

    public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationFailed()");
        foreach (var item in data)
        {
            Debug.Log(item.Key + " " + item.Value.ToString());
        }
    }

    public virtual void OnWebRpcResponse(OperationResponse response)
    {
        Debug.Log("OnWebRpcResponse()" + response);
    }

    public virtual void OnOwnershipRequest(object[] viewAndPlayer)
    {
        Debug.Log("OnOwnershipRequest()");
        foreach (var item in viewAndPlayer)
        {
            Debug.Log(item.ToString());
        }
    }

    public virtual void OnLobbyStatisticsUpdate()
    {
        Debug.Log("OnLobbyStatisticsUpdate()");
    }

    public virtual void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerActivityChanged()" + otherPlayer);
    }

    public virtual void OnOwnershipTransfered(object[] viewAndPlayers)
    {
        Debug.Log("OnOwnershipTransfered()");

    }
    #endregion
}
