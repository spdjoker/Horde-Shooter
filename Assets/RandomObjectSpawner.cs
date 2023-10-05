using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    //Enemies to spawn
    [SerializeField]
    private GameObject Zombie;

    //Spawn intervals
    [SerializeField]
    private float zombieInterval = 5f;

    void Start()
    {//Each enemy needs a coroutine (interval, enemyName)
        zombieInterval = Random.Range(10f, 20f);
        StartCoroutine(spawnEnemy(zombieInterval, Zombie));
    }
    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }

}
