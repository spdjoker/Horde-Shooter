using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GunData", menuName = "Data/Gun Data")]
public class GunData : ScriptableObject
{
    public int damage;
    public float range;
    public float fireRate;
    public GunFlags properties;

    [Flags] public enum GunFlags {
        None = 0x00,
        Automatic = 0x01,   // Hold the button to fire
        // Burst = 0x03,    // Inherently automatic
        // NoReload = 4,    // ???
    }
}
