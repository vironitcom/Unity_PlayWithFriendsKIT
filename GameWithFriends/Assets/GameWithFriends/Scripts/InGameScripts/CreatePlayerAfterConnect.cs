/***************************** Module Header *****************************\
Module Name:  CreatePlayerAfterConnect.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Will create player unit after start scene

The MIT License (MIT)
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\**************************************************************************/
using UnityEngine;

public class CreatePlayerAfterConnect : Photon.MonoBehaviour {

    public CameraController Camera;

    private void Start()
    {
        if(PhotonNetwork.connected)
        {
            CreatePlayerObject();
        }
    }

    void CreatePlayerObject()
    {       
        GameObject newPlayerObject = PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity, 0);
        Camera.Target = newPlayerObject.transform;
    }
}
