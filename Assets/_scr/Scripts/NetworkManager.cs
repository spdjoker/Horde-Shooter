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
    [SerializeField] public int TeamHealth;
    public Hashtable customProperties = new Hashtable();
    public ShopManagerScript healthy;
    public Vector3[] spawnPositions;
    int room = 0;

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

        InitiliazeRoom(room);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    public void InitiliazeRoom(int roomNum)
    {
        Debug.Log(roomNum);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        
        PhotonNetwork.JoinOrCreateRoom(roomNum.ToString(), roomOptions, TypedLobby.Default);
        
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
        
        
        if (PhotonNetwork.IsMasterClient) {
            customProperties.Add("SpawnIndex", 0);
            customProperties.Add("Health", TeamHealth);
            
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

        CheckIfEveryoneReady();
    }

    [PunRPC]
    private void RPC_ChangeReadyState()
    {
        otherPlayerReady = true;
        if(otherPlayerReady)
        {
            readyText.text = "Partner is Ready";
        }else{
            readyText.text = "";
        }

        CheckIfEveryoneReady();

        
    }

    private void CheckIfEveryoneReady(){
        if(!ready || !otherPlayerReady){
            return;
        }

        PhotonNetwork.LoadLevel(1);


        /*if(PhotonNetwork.IsMasterClient){
            XROrigin.transform.position = spawnPositions[0];
        }else{
            XROrigin.transform.position = spawnPositions[1];
        }*/
        
    }

    public void PlayerLoseHealth(){
        customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        TeamHealth = (int)customProperties["Health"] - 1;
        customProperties["Health"] = TeamHealth;
        healthy.HealthTXT.text = "Health:" + TeamHealth.ToString();
        Debug.Log(customProperties["Health"]);

        if((int)customProperties["Health"] <= 0){
            LoseGame();
            return;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
        return;
    }

    public void LoseGame(){
        
        base.photonView.RPC("RPC_ChangeLevel", RpcTarget.MasterClient);
        
        
    }

    [PunRPC]
    private void RPC_ChangeLevel(){
        PhotonNetwork.LoadLevel(2);
    }

}

/*
if player is master
    create hash table

*/