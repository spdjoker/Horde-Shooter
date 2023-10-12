using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject XROrigin;
    public Vector3[] spawnPositions;
    // Start is called before the first frame update
    void Start()
    {
        int spawnIndex = GetComponent<NetworkPlayerSpawner>().spawnIndex;
        Instantiate(XROrigin, spawnPositions[spawnIndex], Quaternion.identity);
        GetComponent<NetworkPlayerSpawner>().spawnIndex = spawnIndex + 1;
        Debug.Log(GetComponent<NetworkPlayerSpawner>().spawnIndex);
    }

    

}
