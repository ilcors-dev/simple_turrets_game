using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    /// <summary>
    /// The sub-waves to spawn in each wave
    /// </summary>
    public SubWave[] subWaves;

    public Wave(SubWave[] sub)
    {
        subWaves = sub;
    }
}
