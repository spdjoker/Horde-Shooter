using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;



public class PlayerSpawner : MonoBehaviour
{
    public GameObject XROrigin;
    public Vector3[] spawnPositions;
    private int spawnIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        XROrigin.transform.position = spawnPositions[spawnIndex];
        updateSpawnIndex();
    
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(spawnIndex);
        }
        else if (stream.IsReading)
        {
            updateSpawnIndex();
            Debug.Log("test");
        }
    }

    private void updateSpawnIndex()
    {
        spawnIndex++;
    }

    

}
