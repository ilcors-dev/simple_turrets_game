using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubWave
{
    /// <summary>
    /// The enemy that will get spawned this wave
    /// </summary>
    public GameObject enemy;

    /// <summary>
    /// Enemies to spawn
    /// </summary>
    public int count;

    /// <summary>
    /// Spawn rate of the wave enemies
    /// </summary>
    [Tooltip("Spawn every 1/rate, the higher rate is the more enemies in less time will spawn")]
    public float rate;

    public SubWave(GameObject enemy, int count, float rate)
    {
        this.enemy = enemy;
        this.count = count;
        this.rate = rate;
    }
}