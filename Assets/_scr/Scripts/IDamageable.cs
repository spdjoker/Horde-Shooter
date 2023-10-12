using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(int amount);
    public void Heal(int amount);
    public void Drop();
}
