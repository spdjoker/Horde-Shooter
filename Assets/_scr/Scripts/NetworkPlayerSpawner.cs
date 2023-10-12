using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public Vector3[] spawnLocation;
    private int spawnIndex = 0;
    private GameObject playerBody;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        playerBody = PhotonNetwork.Instantiate("Network Player", spawnLocation[spawnIndex], transform.rotation);
        spawnIndex++;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(playerBody);
    }
}
