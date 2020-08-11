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
    public float rate;
}