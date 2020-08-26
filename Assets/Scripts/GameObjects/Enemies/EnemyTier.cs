using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTier
{
    public int tier { get; }

    public float baseHealth { get; }

    public float baseSpeed { get; }

    public EnemyTier(int tier, float baseHealth, float baseSpeed)
    {
        this.tier = tier;
        this.baseHealth = baseHealth;
        this.baseSpeed = baseSpeed;
    }
}
