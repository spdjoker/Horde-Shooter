using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviourPun
{
    //Enemies to spawn
    [SerializeField]
    private GameObject zombie;
    private GameObject skeleton;
    private GameObject spider;

    private bool spawning = false;

    [SerializeField] private float baseInterval = 1f;
    [SerializeField] private float zombieInterval = 5f;
    [SerializeField] private float skeletonInterval = 10f;
    [SerializeField] private float spiderInterval = 15f;
    [SerializeField] private float randomIntervalRange = 1f;
    [SerializeField] private float spawnPositionVariance = 3f;


    //Roll a random number from 0 - 100 and return a random enemy's interval
    //Roll a random number from 0 - 4 and return a random enemy object

    //THIS CODE HAS NOT BEEN TESTED. BEWARE.

    float intervalRandomizer(){
        float range = Random.Range(0, 101);
        if(range < 25){
            return zombieInterval;
        }else if (range < 75){
            return skeletonInterval;
        }else {//Else range < 100
            return spiderInterval;
        }
    }
    GameObject enemyRandomizer(){
        float range = Random.Range(0, 5);
        if(range == 0){
            return zombie;
        }else if (range == 1){
            return skeleton;
        }else {//Else range is 2, 3 or 4
            return spider;
        }
    }
    void Update()
    {//Each enemy needs a coroutine (interval, enemyName)
        if (photonView.IsMine && PhotonNetwork.IsConnected && !spawning)
        {
            spawning = true;
            StartCoroutine(SpawnEnemy(RandomInterval(), zombie));
        }
    }
    //Original: private float RandomInterval() => Random.Range(zombieInterval - randomIntervalRange, zombieInterval + randomIntervalRange);
    private float RandomInterval() => Random.Range(baseInterval - randomIntervalRange,  baseInterval + randomIntervalRange);

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        float varX = Random.Range(-spawnPositionVariance, spawnPositionVariance);
        float varY = Random.Range(-spawnPositionVariance, spawnPositionVariance);
        Vector3 position = new Vector3(transform.position.x + varX, 5f, transform.position.z + varY);
        //int numPlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["SpawnIndex"] + 1;
        for (int i = 0; i < 2; i++)
        {
            Debug.Log("i:" + i.ToString());
            PhotonNetwork.InstantiateRoomObject(i == 0 ? "Red Enemy" : "Blue Enemy", position, Quaternion.identity);
        }
        spawning = false;
    }

}
