/***************************** Module Header *****************************\
Module Name:  WaitingForFriendHelper.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Script that will be on popup that intave friend to play.

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\**************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class InvateHelper : MonoBehaviour
{
    [SerializeField] Image avatar;
    [SerializeField] Text title;
    [SerializeField] Button accept;
    [SerializeField] Button decline;
    [SerializeField] ChatHelper chatHelper;

    private void OnEnable()
    {
        string apiAvatar = string.Format("/{0}/picture?type=square&height=128&width=128", chatHelper.InvatedFriendID);
        FB.API(apiAvatar, HttpMethod.GET, setAvatar);
        string apiName = string.Format("/{0}?fields=name", chatHelper.InvatedFriendID);
        FB.API(apiName, HttpMethod.GET, setName);
        accept.onClick.AddListener(DoAccept);
        decline.onClick.AddListener(DoDecline);
    }

    private void OnDisable()
    {
        accept.onClick.RemoveAllListeners();
        decline.onClick.RemoveAllListeners();
    }
    private void setName(IResult result)
    {
        if (result.Error == null)
        {
            title.text = string.Format("{0} WANTS TO PLAY", result.ResultDictionary["name"].ToString());
        }
    }
    private void setAvatar(IGraphResult result)
    {
        if (result.Texture != null)
        {
            avatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
        }
    }

    private void DoAccept()
    {
        ActionManager.Action_OnPrivateMessageSend(chatHelper.InvatedFriendID, "ok");
    }

    private void DoDecline()
    {
        ActionManager.Action_OnPrivateMessageSend(chatHelper.InvatedFriendID, "no");
    }
}

