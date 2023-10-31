using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;


public class NetworkManager : MonoBehaviourPunCallbacks
{

    public GameObject XROrigin;
    Hashtable customProperties = new Hashtable();
    public Vector3[] spawnPositions;
    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
    }

    // Update is called once per frame
    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        
        Debug.Log("Try Connecting To Server");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server!");
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
        
        if (PhotonNetwork.IsMasterClient) {
            customProperties.Add("SpawnIndex", 0);
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
        }
        else
        {
            int spawnpoint = (int)PhotonNetwork.CurrentRoom.CustomProperties["SpawnIndex"] + 1;
            customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            customProperties["SpawnIndex"] = spawnpoint;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
            XROrigin.transform.position = spawnPositions[spawnpoint];
        }


        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }
}

/*
if player is master
    create hash table

*/