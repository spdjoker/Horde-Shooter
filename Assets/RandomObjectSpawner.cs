using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct MonsterData {
    public GameObject prefab;
    public float spawnInterval;
}
public class RandomObjectSpawner : MonoBehaviour
{
    //Enemies to spawn
    [SerializeField] private List <MonsterData> mobs = new List<MonsterData>(5);
    [SerializeField] private float randomIntervalRange = 1f;
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
        StartCoroutine(SpawnEnemy(RandomInterval(), enemyRandomizer()));
    }
    //Original: private float RandomInterval() => Random.Range(zombieInterval - randomIntervalRange, zombieInterval + randomIntervalRange);
    private float RandomInterval() => Random.Range(intervalRandomizer() - randomIntervalRange,  intervalRandomizer() + randomIntervalRange);
    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        float varX = Random.Range(-spawnPositionVariance, spawnPositionVariance);
        float varY = Random.Range(-spawnPositionVariance, spawnPositionVariance);
        Vector3 position = new Vector3(transform.position.x + varX, 1.0f, transform.position.z + varY);
        Instantiate(enemy, position, Quaternion.identity);
        StartCoroutine(SpawnEnemy(RandomInterval(), enemyRandomizer()));
    }

}
