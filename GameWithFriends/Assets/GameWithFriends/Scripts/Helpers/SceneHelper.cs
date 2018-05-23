/***************************** Module Header *****************************\
Module Name:  SceneHelper.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Menu scene controller. Just simple methods in one place. 

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\**************************************************************************/
using UnityEngine;

public class SceneHelper : MonoBehaviour
{
    [SerializeField] GameObject FindRandomPanel;
    [SerializeField] GameObject CreateRoomPanel;
    [SerializeField] GameObject LeaveRoomPanel;
    [SerializeField] GameObject ListOfRoomPanel;
    [SerializeField] GameObject FriendListPanel;
    [SerializeField] UnityEngine.UI.Button LoginFacebookButton;

    private void Start()
    {
        ActionManager.Action_OnLeaveRoom += hideLeavePanel;
        ActionManager.Action_OnJoinedRoom += hideCreateAndRandomPanel;
        ActionManager.Action_OnFacebookLogin += turnOffLoginFacebookButton;

        LeaveRoomPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        ActionManager.Action_OnLeaveRoom -= hideLeavePanel;
        ActionManager.Action_OnJoinedRoom -= hideCreateAndRandomPanel;
        ActionManager.Action_OnFacebookLogin -= turnOffLoginFacebookButton;
    }

    private void hideCreateAndRandomPanel()
    {
        FindRandomPanel.SetActive(false);
        CreateRoomPanel.SetActive(false);
        LeaveRoomPanel.SetActive(true);
        ListOfRoomPanel.SetActive(false);
        FriendListPanel.SetActive(false);
    }

    private void hideLeavePanel()
    {
        FindRandomPanel.SetActive(true);
        CreateRoomPanel.SetActive(true);
        LeaveRoomPanel.SetActive(false);
        ListOfRoomPanel.SetActive(true);
        FriendListPanel.SetActive(true);
    }

    private void turnOffLoginFacebookButton(Facebook.Unity.IResult result) 
    {
        LoginFacebookButton.interactable = false;
    }

    public void OnButtonClick_DoJoinRandomRoom(int playersAmount)
    {
        ActionManager.Action_DoJoinRandomRoom((byte)playersAmount);
    }

    public void OnButtonClick_DoCreateRoom(int playersAmount)
    {
        ActionManager.Action_DoCreateRoom((byte)playersAmount);
    }

    public void OnButtonClick_DoLeaveRoom()
    {
        ActionManager.Action_DoLeaveRoom();
    }

    public void OnButtonClick_DoLoginWithFacebook()
    {
        ActionManager.Action_DoLoginWithFacebook();
    }
}
