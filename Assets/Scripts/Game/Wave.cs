using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [Header("The subwaves to spawn")]
    /// <summary>
    /// The sub-waves to spawn in each wave
    /// </summary>
    public SubWave[] subWaves;

    [Header("The wave difficulty")]
    /// <summary>
    /// The current wave difficulty
    /// </summary>
    [SerializeField]
    public float waveDifficulty = 1;

    /// <summary>
    /// Signals if the wave is finished
    /// </summary>
    public bool finished = false;

    public Wave(SubWave[] sub)
    {
        finished = false;
        subWaves = sub;
    }

    public Wave(float waveDifficulty)
    {
        finished = false;
        this.waveDifficulty = waveDifficulty;
    }
}
