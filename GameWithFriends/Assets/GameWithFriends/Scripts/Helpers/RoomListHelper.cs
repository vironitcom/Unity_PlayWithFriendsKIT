/***************************** Module Header *****************************\
Module Name:  RoomListHelper.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Help us to update list of rooms by action 

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\**************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class RoomListHelper : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] GameObject JoinRoomButtonPrefab;

    private void Start()
    {
        ActionManager.Action_OnRoomListUpdate += DoCreateRoomList;
    }

    private void OnDestroy()
    {
        ActionManager.Action_OnRoomListUpdate -= DoCreateRoomList;
    }
    public void DoCreateRoomList(RoomInfo[] allRooms)
    {
        foreach(var item in FindObjectsOfType<JoinRoomItemHelper>())
        {
            Destroy(item.gameObject);
        }

        foreach (var item in allRooms)
        {
            string roomName = item.Name;
            GameObject but = Instantiate(JoinRoomButtonPrefab, content) as GameObject;
            but.GetComponentInChildren<Text>().text = roomName + " join";
            but.GetComponentInChildren<Button>().onClick.AddListener(() => { PhotonNetwork.JoinRoom(roomName); });
        }
    }
}
