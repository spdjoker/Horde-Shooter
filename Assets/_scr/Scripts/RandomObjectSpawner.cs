using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using System.IO;

[System.Serializable]
public struct MonsterData {
    public string prefab;
    public float spawnInterval;
}
public class RandomObjectSpawner : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    //Enemies to spawn
    [SerializeField] private List <MonsterData> mobs = new List<MonsterData>(5);
    [SerializeField] private float interval;
    [SerializeField] private GameObject skull;
    [SerializeField] private float delay;
    [SerializeField] private Vector3[] enemySpawnPoints;
    [SerializeField] private NetworkManager networkManager;

    //[SerializeField] private float randomIntervalRange = 1f;

    //Roll a random number from 0 - 100 and return a random enemy's interval
    //Roll a random number from 0 - 4 and return a random enemy object

    //THIS CODE HAS NOT BEEN TESTED. BEWARE.

    string enemyRandomizer() {
        if (mobs.Count >= 3) {
            float range = Random.Range(0, 5);
            if (range == 0) {
                return mobs[0].prefab;
            } else if (range == 1) {
                return mobs[1].prefab;
            } else {
                return mobs[2].prefab;
            }
        } else {
            //When there are not enough elements in the list
            return null;
        }
    }
    //ssss
    void Start()
    {//Each enemy needs a coroutine (interval, enemyName)
        
        StartCoroutine(SpawnEnemy(interval, "SKELETON", delay));
        
    }
    private float RandomInterval() => Random.Range(interval - 1,  interval + 1);
   
    private IEnumerator SpawnEnemy(float interval, string enemy, float wait)
    {
        yield return new WaitForSeconds(interval + wait);
        
        if(PhotonNetwork.IsMasterClient){
            int coin = Random.Range(0, 2);
            int position = Random.Range(0, 3);
            
            if(coin == 0)
            {
                photonView.RPC("RPC_SpawnEnemyMaster", RpcTarget.MasterClient, enemy, position);
            }else{
                photonView.RPC("RPC_SpawnEnemyOther", RpcTarget.OthersBuffered, enemy, position);
            }
        }
        
        

        /*if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Instantiate("BLUE_" + enemy, position, Quaternion.identity, 0);
        }else{
            PhotonNetwork.Instantiate("RED_" + enemy, position, Quaternion.identity, 0);
        }*/
        
        StartCoroutine(SpawnEnemy(interval, "SKELETON", delay));
        
    }
    [PunRPC]
    private void RPC_SpawnEnemyMaster(string enemy, int position)
    {

        PhotonNetwork.Instantiate("BLUE_" + enemy, enemySpawnPoints[position], Quaternion.identity, 0);
        
    }

    [PunRPC]
    private void RPC_SpawnEnemyOther(string enemy, int position)
    {

        PhotonNetwork.Instantiate("RED_" + enemy, enemySpawnPoints[position], Quaternion.identity, 0);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // Access the instantiated GameObject using info.photonView
        GameObject instantiatedObject = info.photonView.gameObject;
        Debug.Log(info.Sender);

        // Now you have a reference to the instantiated GameObject
        // You can do whatever you need with it
    }

}




/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class RandomObjectSpawner : MonoBehaviour
{
    //Enemies to spawn
    [SerializeField]
    private GameObject zombie;
    private GameObject skeleton;
    private GameObject spider;

    private float baseInterval = 10f;
    [SerializeField] private float zombieInterval = 5f;
    [SerializeField] private float skeletonInterval = 10f;
    [SerializeField] private float spiderInterval = 15f;
    private float randomIntervalRange = 5f;
    [SerializeField] private float spawnPositionVariance = 3f;

    //Roll a random number from 0 - 100 and return a random enemy's interval
    //Roll a random number from 0 - 4 and return a random enemy object

    //THIS CODE HAS NOT BEEN TESTED. BEWARE.

    float intervalRandomizer() {
        if (mobs.Count >= 3) {
            float range = Random.Range(0, 101);
            if (range < 25) {
                return mobs[0].spawnInterval;
            } else if (range < 75) {
                return mobs[1].spawnInterval;
            } else {
                return mobs[2].spawnInterval;
            }
        } else {
            return 0.0f;
        }
    }
    GameObject enemyRandomizer() {
        if (mobs.Count >= 3) {
            float range = Random.Range(0, 5);
            if (range == 0) {
                return mobs[0].prefab;
            } else if (range == 1) {
                return mobs[1].prefab;
            } else {
                return mobs[2].prefab;
            }
        } else {
            //When there are not enough elements in the list
            return null;
        }
    }
    
    void Start()
    {//Each enemy needs a coroutine (interval, enemyName)
        StartCoroutine(SpawnEnemy(RandomInterval(), zombie));
    }
    //Original: private float RandomInterval() => Random.Range(zombieInterval - randomIntervalRange, zombieInterval + randomIntervalRange);
    private float RandomInterval() => Random.Range(intervalRandomizer() - randomIntervalRange,  intervalRandomizer() + randomIntervalRange);
    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        float varX = Random.Range(-spawnPositionVariance, spawnPositionVariance);
        float varY = Random.Range(-spawnPositionVariance, spawnPositionVariance);
        Vector3 position = new Vector3(transform.position.x + varX, 5f, transform.position.z + varY);
        //Instantiate(enemy, position, Quaternion.identity);
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Instantiate("BLUE_SKELETON", position, Quaternion.identity, 0);
        }else{
            PhotonNetwork.Instantiate("RED_SKELETON", position, Quaternion.identity, 0);
        }
            
        StartCoroutine(SpawnEnemy(RandomInterval(), enemy));

    }

}
*/