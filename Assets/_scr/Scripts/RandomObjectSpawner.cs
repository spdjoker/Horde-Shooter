using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    //Enemies to spawn
    [SerializeField]
    private GameObject zombie;

    //Spawn intervals
    [SerializeField] private float zombieInterval = 5f;
    [SerializeField] private float randomIntervalRange = 1f;
    [SerializeField] private float spawnPositionVariance = 3f;

    void Start()
    {//Each enemy needs a coroutine (interval, enemyName)
        StartCoroutine(SpawnEnemy(RandomInterval(), zombie));
    }

    private float RandomInterval() => Random.Range(zombieInterval - randomIntervalRange, zombieInterval + randomIntervalRange);

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        float varX = Random.Range(-spawnPositionVariance, spawnPositionVariance);
        float varY = Random.Range(-spawnPositionVariance, spawnPositionVariance);
        Vector3 position = new Vector3(transform.position.x + varX, 1.0f, transform.position.z + varY);
        Instantiate(enemy, position, Quaternion.identity);
        StartCoroutine(SpawnEnemy(RandomInterval(), enemy));
    }

}
