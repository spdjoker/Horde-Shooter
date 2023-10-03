using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public GameObject[] myObjects;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))//Upon pressing spacebar, an asset (cylinder) should spawn
        {
            int randomIndex = Random.Range(0, myObjects.Length);//Link the random spawn positions to the Position and Scale of the Spawner, also make more spawners.
            //                                                  This way, the spawning will be linked to the area of the boxes rather than the hardcoded random values down there vv
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 11), 5, Random.Range(-10, 11));
            //                                        Vector(x,y,z)

            Instantiate(myObjects[randomIndex], randomSpawnPosition, Quaternion.identity);
        }
        
    }
}
