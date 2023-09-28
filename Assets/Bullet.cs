using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public BulletData data;
    private Vector3 originalPosition;

    public Rigidbody rb;

    public void Update()
    {
        if (Vector3.Distance(originalPosition, transform.position) > data.range)
        {
            Destroy(this);
        }
    }

    public static void Fire(Vector3 from, Vector3 direction, BulletData data)
    {
        Bullet bullet = Instantiate(data.model, from, Quaternion.identity).GetComponent<Bullet>();
        bullet.transform.LookAt(direction);
        bullet.data = data;
    }
}