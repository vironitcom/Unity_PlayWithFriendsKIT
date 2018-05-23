/***************************** Module Header *****************************\
Module Name:  ActionManager.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

This is static class that will able from any part of application and will be used for invoice Actions
(()=>{}) or ((<T>) => {}) will protect us from any null, and we can simple call action.

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
using System;
using System.Collections.Generic;

//this is static class that will able from any part of application and will be used for invoice Actions
// (()=>{}) or ((<T>) => {}) will protect us from any null, and we can simple call action.
public static class ActionManager
{    
    #region FACEBOOK
    public static Action<Facebook.Unity.IResult> Action_OnFacebookLogin = new Action<Facebook.Unity.IResult>((FacebookLoginResult) => { });
    public static Action Action_DoLoginWithFacebook = new Action(() => { });
    #endregion
    #region GAME_WITH_FRIENDS_ACTIONS
    public static Action<List<string>,Dictionary<string, string>, string> Action_OnChatConnect = new Action<List<string>, Dictionary<string, string>, string>((friendsIDList, friendsDictionary, UserID) => { });
    public static Action<string, string> Action_OnPrivateMessageSend = new Action<string, string>((friendId, message) => { });
    public static Action<string, object> Action_OnGetPrivateMessage = new Action<string, object>((sender, message) => { });
    public static Action<string, int> Action_OnUsersStatusUpdate = new Action<string, int>((user, status) => { });
    public static Action Action_OnChatDisconnect = new Action(() => { });
    public static Action<string> Action_OnInvateFriend = new Action<string>((friendId) => { });
    #endregion
    #region PHOTON_CALLBACK_ACTIONS
    public static Action Action_OnLeaveRoom = new Action(() => { });
    public static Action Action_OnJoinedRoom = new Action(() => { });
    public static Action Action_OnCreatedRoom = new Action(() => { });
    public static Action Action_OnPhotonPlayerConnected = new Action(() => { });
    public static Action Action_DoLeaveRoom = new Action(() => { });
    public static Action<byte> Action_DoJoinRandomRoom = new Action<byte>((PlayersAmount) => { });
    public static Action<byte> Action_DoCreateRoom = new Action<byte>((PlayersAmount) => { });
    public static Action<string> Action_DoCreateFriendRoom = new Action<string>((FriendID) => { });
    public static Action<string> Action_DoJoinFriendRoom = new Action<string>((FriendID) => { });
    public static Action<RoomInfo[]> Action_OnRoomListUpdate = new Action<RoomInfo[]>((RoomInfoList) => { });
    #endregion
}
