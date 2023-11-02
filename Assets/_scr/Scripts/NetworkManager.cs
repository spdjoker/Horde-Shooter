using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using TMPro;


public class NetworkManager : MonoBehaviourPunCallbacks
{

    public GameObject XROrigin;
    [SerializeField] TMP_Text readyText;
    Hashtable customProperties = new Hashtable();
    public Vector3[] spawnPositions;

    private bool ready = false; 
    private bool otherPlayerReady = false;



    void Start()
    {
        ConnectToServer();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

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
            XROrigin.transform.position = spawnPositions[0];
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

    public void OnClick_ReadyUp()
    {
        ready = !ready;
       
        
        base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.OthersBuffered);

        checkIfEveryoneReady();
    }

    [PunRPC]
    private void RPC_ChangeReadyState()
    {
        otherPlayerReady = !otherPlayerReady;
        if(otherPlayerReady)
        {
            readyText.text = "Partner is Ready";
        }else{
            readyText.text = "";
        }

        checkIfEveryoneReady();
        
    }

    private void checkIfEveryoneReady(){
        if(!ready || !otherPlayerReady){
            return;
        }

        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel(1);
            base.photonView.RPC("RPC_FixPositioning", RpcTarget.All);
            XROrigin.transform.position = spawnPositions[0];
        }
    }
    [PunRPC]
    private void RPC_FixPositioning(){
        if(PhotonNetwork.IsMasterClient){
            XROrigin.transform.position = spawnPositions[0];
        }else{
            XROrigin.transform.position = spawnPositions[1];
        }
    }
}

/*
if player is master
    create hash table

*/