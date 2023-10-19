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
    public TargetFags teamFlags;

    [Flags] public enum TargetFags {
        Gem = 1,
        Players = 2,
    }  
    
}
