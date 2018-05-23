/***************************** Module Header *****************************\
Module Name:  WaitingForFriendHelper.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Script that will be on popup while we wait for answer from friend.

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\**************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class WaitingForFriendHelper : MonoBehaviour
{
    [SerializeField] Image avatar;
    [SerializeField] Text title;
    [SerializeField] Button CancelButton;
    [SerializeField] ChatHelper chatHelper;

    private void OnEnable()
    {
        string apiAvatar = string.Format("/{0}/picture?type=square&height=128&width=128", chatHelper.SelectedFriendID);
        FB.API(apiAvatar, HttpMethod.GET, setAvatar);
        string apiName = string.Format("/{0}?fields=name", chatHelper.SelectedFriendID);
        FB.API(apiName, HttpMethod.GET, setName);
        CancelButton.onClick.AddListener(DoCancel);
    }

    private void OnDisable()
    {
        CancelButton.onClick.RemoveAllListeners();
    }
    private void setName(IResult result)
    {
        if (result.Error == null)
            title.text = string.Format("WAITING FOR {0}", result.ResultDictionary["name"].ToString());
    }
    private void setAvatar(IGraphResult result)
    {
        if (result.Texture != null)
            avatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
    }
    private void DoCancel()
    {
        ActionManager.Action_OnPrivateMessageSend(chatHelper.SelectedFriendID, "cancel");
    }
}

