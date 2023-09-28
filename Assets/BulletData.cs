using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BulletData", menuName = "Data/Bullet Data")]
public class BulletData : ScriptableObject
{
    public uint damage;
    public float velocity;
    public float range;

    public GameObject model;
}
