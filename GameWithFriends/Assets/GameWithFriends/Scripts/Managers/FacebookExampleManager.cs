/***************************** Module Header *****************************\
Module Name:  FacebookExampleManager.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Simple example for facebook login

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\**************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookExampleManager : MonoBehaviour
{
    private void Awake()
    {
        InitFacebook();
    }

    private void Start()
    {
        ActionManager.Action_DoLoginWithFacebook += LoginWithFacebook;
    }
    private void OnDestroy()
    {
        ActionManager.Action_DoLoginWithFacebook -= LoginWithFacebook;
    }

    private void InitFacebook()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();            
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
            Time.timeScale = 0;

        else
            Time.timeScale = 1;
    }

    private void LoginWithFacebook()
    {
        Debug.Log("LOgin With Facebook");
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            FB.API("me?fields=id,friends", HttpMethod.GET, ((LoginResult) => { ActionManager.Action_OnFacebookLogin(LoginResult); }));
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }    
}
