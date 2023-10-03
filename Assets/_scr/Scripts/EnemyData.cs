using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int startHealth;
    public float speed;
    public float range;
    public int goldOnDrop;
    public TeamFlags teamFlags;

    [Flags] public enum TeamFlags {
        Red = 1,
        Green = 2,
        Blue = 4,
    }  
    
}
