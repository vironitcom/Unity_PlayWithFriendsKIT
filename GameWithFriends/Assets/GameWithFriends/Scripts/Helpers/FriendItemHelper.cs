/***************************** Module Header *****************************\
Module Name:  FriendItemHelper.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Script that will be on prefab and will help it works 

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\**************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FriendItemHelper : MonoBehaviour
{
    [SerializeField] private Image AvatarImage;
    [SerializeField] private Image statusImage;
    [SerializeField] private Button SendInvateButton;
    public string FriendId { get; set; }
    public Text NameLabel;
    string userStatus;
    /// <summary>
    /// Set-up this iten
    /// </summary>
    /// <param name="id">FacebookID</param>
    public void CreateFriend(string id)
    {
        SendInvateButton.onClick.AddListener(invateFriend);
        string apiAvatar = string.Format("/{0}/picture?type=square&height=128&width=128", FriendId);
        FB.API(apiAvatar, HttpMethod.GET, setAvatar);
        string apiName = string.Format("/{0}?fields=name", FriendId);
        FB.API(apiName, HttpMethod.GET, setName);
        statusImage.color = Color.red;
    }

    private void Update()
    {
        SendInvateButton.interactable = userStatus == "Online";
    }

    private void invateFriend()
    {
        ActionManager.Action_OnInvateFriend(FriendId);
        ActionManager.Action_OnPrivateMessageSend(FriendId, "invate");
    }

    private void setName(IResult result)
    {
        if (result.Error == null)
        {
            NameLabel.text = result.ResultDictionary["name"].ToString();
        }
    }

    private void setAvatar(IGraphResult result)
    {
        if (result.Texture != null)
        {
            AvatarImage.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
        }
    }

    public void OnFriendStatusUpdate(int status)
    {
        string _status;

        switch (status)
        {
            case 1:
                _status = "Invisible";
                break;
            case 2:
                _status = "Online";
                statusImage.color = Color.green;
                break;
            case 3:
                _status = "Away";
                break;
            case 4:
                _status = "Do not disturb";
                break;
            case 5:
                _status = "Looking For Game/Group";
                break;
            case 6:
                _status = "Playing";
                statusImage.color = Color.yellow;
                break;
            default:
                _status = "Offline";
                statusImage.color = Color.red;
                break;
        }
        userStatus = _status;
    }
}
