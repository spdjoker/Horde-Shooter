using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private Vector3 spawnLocation;
    private GameObject playerBody;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        playerBody = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(playerBody);
    }
}
