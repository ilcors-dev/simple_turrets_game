using System.Collections;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    /// <summary>
    /// The current enemies alive
    /// </summary>
    public static int EnemiesAlive = 0;

    public Wave[] waves;

    /// <summary>
    /// Should the spawn of enemy start?
    /// </summary>
    private bool startSpawn;

    [SerializeField]
    ///<summary>The time from a wave to the other</summary>
    private float timeBetweenWaves;

    private float countdown = 0;

    /// <summary>
    /// The spawned waves
    /// </summary>
    private int waveIndex = 0;

    /// <summary>
    /// The enemy spawn position
    /// </summary>
    public Vector3 spawnPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        countdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        // do not spawn if the startSpawn flag is false and there are enemies alive
        if (!startSpawn || EnemiesAlive > 0)
            return;

        // if the countdown of the spawn is <= 0, we can start spawining enemies
        if (countdown <= 0f)
        {
            StartCoroutine(StartWave());
            countdown = timeBetweenWaves;// set the countdown equal to the time between waves
            return;
        }

        // decrease the countdown
        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        
        // update next wave ui text
        UIManager.Instance.SetNextWaveCountdownText(countdown);
    }

    /// <summary>
    /// Starts the enemy spawning
    /// </summary>
    public void StartEnemySpawn()
    {
        startSpawn = true;
    }

    /// <summary>
    /// Starts the spawning of a new wave
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartWave()
    {
        // round increase
        GameManager.Instance.round++;

        // set text ui
        UIManager.Instance.SetRoundIndexText();

        // get the wave to spawn
        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        // the number of enemies to spawn each wave
        waveIndex++;
    }

    /// <summary>
    /// Spawns an enemy
    /// </summary>
    private void SpawnEnemy(GameObject enemy)
    {
        GameObject spawned = Instantiate(enemy, spawnPosition, Quaternion.identity);
        spawned.SetActive(true);
        EnemiesAlive++;
    }
}
