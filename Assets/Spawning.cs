using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawning : MonoBehaviour
{

    public Transform plane;
    public GameObject spawnablePrefab;


    float x_dim;
    float z_dim;

    // Start is called before the first frame update
    void Start()
    {
        x_dim = plane.size.x;
        z_dim = plane.size.z;
    }

    // Update is called once per frame
    void Spawn()
    {
        GameObject obj = Instantiate (Spawner, Vector3.zero, Qauternion.Identity, plane) as GameObject;

        var x_rand =  Random.Range(-x_dim, x_dim);
	    var z_rand =  Random.Range(-z_dim, z_dim);

        // Since the object is a child of the plane it will automatically handle rotational offset
	    obj.transform.position = new Vector3 (x_rand, y_rand, z_rand);

        //Must be done at the end?
        obj.parent = null;
    }
}
