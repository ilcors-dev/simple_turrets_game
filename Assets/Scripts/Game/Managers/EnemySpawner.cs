using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    /// <summary>
    /// The current enemies alive
    /// </summary>
    public static int EnemiesAlive = 0;

    public List<Wave> waves;

    /// <summary>
    /// Should the spawn of enemy start?
    /// </summary>
    private bool startSpawn;

    /// <summary>
    /// The spawned waves
    /// </summary>
    private int waveIndex = 0;

    /// <summary>
    /// The enemy spawn position
    /// </summary>
    public Vector3 spawnPosition { get; set; }

    [Header("Spawn properties")]
    /// <summary>
    /// Signals if the level is an endless one.
    /// If so the waves will get built during the course of the round
    /// </summary>
    [SerializeField]
    private bool endless;

    [SerializeField]
    ///<summary>The time from a wave to the other</summary>
    private float timeBetweenWaves;

    private float countdown = 0;

    [SerializeField]
    /// <summary>
    /// The enemies that can be spawned
    /// </summary>
    private GameObject[] availableEnemies;

    /// <summary>
    /// How much the generated wave difficulties will be incremented
    /// </summary>
    [SerializeField]
    [Tooltip("How much the generated wave difficulties will be incremented (%)")]
    private int difficultyIncrement;

    // Start is called before the first frame update
    void Start()
    {
        EnemiesAlive = 0;
        countdown = timeBetweenWaves;

        //availableEnemies = GetAvailableEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        // do not spawn if the startSpawn flag is false and there are enemies alive
        if (!startSpawn || EnemiesAlive > 0)
            return;
        // current wave is not yet finished, return
        if (waveIndex != 0)
            if(!waves[waveIndex - 1].finished)
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

        /*
         * get the waves in the wave to spawn.
         * A wave can spawn multiple waves!
         */
        Wave waveWaves = waves[waveIndex];

        for (int i = 0; i < waveWaves.subWaves.Length; i++)
        {
            // get the wave of the wave
            SubWave wave = waveWaves.subWaves[i];
            for (int j = 0; j < wave.count; j++)
            {
                SpawnEnemy(wave.enemy);
                yield return new WaitForSeconds(1f / wave.rate);
            }
        }

        // signal that the wave is finished
        waveWaves.finished = true;

        // the number of enemies to spawn each wave
        waveIndex++;

        if (waveIndex >= waves.Count)
        {
            BuildNewWave();
        }
    }

    /// <summary>
    /// Spawns an enemy
    /// </summary>
    private void SpawnEnemy(GameObject enemy)
    {
        EnemiesAlive++;
        GameObject spawned = Instantiate(enemy, spawnPosition, Quaternion.identity);
        spawned.SetActive(true);
    }

    /// <summary>
    /// Builds a new wave based on the previous spawned
    /// </summary>
    private void BuildNewWave()
    {
        Wave lastWave = waves[waves.Count - 1];

        // the previous wave difficulty
        float previousDifficulty = lastWave.waveDifficulty;

        // create new wave with an incremented difficulty -> prev + prev * (difficulty increment / 100)
        Wave newWave = new Wave(previousDifficulty + (previousDifficulty * (difficultyIncrement / 100)));

        // increment the subwaves count to the new wave
        //if (Random.Range(0, 2) == 1)
        //    newWave.subWaves = new SubWave[lastWave.subWaves.Length + 1];
        //else
        //    newWave.subWaves = new SubWave[lastWave.subWaves.Length];

        // add the subwaves to the wave
        newWave.subWaves = BuildNewSubWaves(lastWave, newWave.waveDifficulty, Random.Range(0, 50) < 15).ToArray();

        Debug.Log(newWave.subWaves.Length);

        // add the newly created wave to the waves list
        waves.Add(newWave);
    }

    /// <summary>
    /// Builds new subwaves based on the last wave and the new difficulty
    /// </summary>
    /// <param name="lastWave">The last wave to 'copy' & buff</param>
    /// <param name="newDifficulty">The difficulty the new will have</param>
    /// <param name="addSubWave">Signals if the subwaves array length should be incremented compared to the last wave</param>
    /// <returns></returns>
    private List<SubWave> BuildNewSubWaves(Wave lastWave, float newDifficulty, bool addSubWave)
    {

        // the subwaves that will get generated below
        List<SubWave> newSubWaves = new List<SubWave>();

        // create new subwaves based on the last contained in the last wave
        foreach (SubWave toCopySubwave in lastWave.subWaves)
        {
            
            // add the new built subwave into the subwaves list
            newSubWaves.Add(CreateSubWave(toCopySubwave, newDifficulty));
        }

        // increment the subwaves length by one with a new build new subwave
        if (addSubWave)
            newSubWaves.Add(CreateSubWave(lastWave.subWaves[Random.Range(0, lastWave.subWaves.Length)], newDifficulty));

        return newSubWaves;
    }

    /// <summary>
    /// Creates a new subwave containing a random enemy of a given tier and buffs it based on a float value representing the new wave difficulty.
    /// </summary>
    /// <param name="lastSubWave">The last subwave that we will 'copy' / change a bit</param>
    /// <param name="newDifficulty">The new wave difficulty, the enemies in the new subwave will get buffed based on this</param>
    /// <returns></returns>
    private SubWave CreateSubWave(SubWave lastSubWave, float newDifficulty)
    {
        // get a random enemy by tier
        GameObject toBuff = GetRandomEnemyByTier(lastSubWave.enemy.GetComponent<Enemy>().tier);

        // get its script
        Enemy enemy = toBuff.GetComponent<Enemy>();

        // buff it to make it stronger than the last wave
        enemy.Health += (int)(enemy.Health * (newDifficulty / 60*2));
        enemy.WorthCoinValue += enemy.WorthCoinValue * (int)(newDifficulty / 100);

        // get the enemies count compared to the new difficulty percentage
        int subWaveCountPercentage = (int)(lastSubWave.count * newDifficulty / 100);

        // should the new wave have more or less enemies?
        int addCount = Random.Range(lastSubWave.count - subWaveCountPercentage,
            lastSubWave.count + subWaveCountPercentage);

        // create new subwave with the picked enemey, the count of the enemy that will get spawned
        // and the rate at which each enemy will get spawned based on previous rate and current wave difficulty

        float spawnRatePercentage = (lastSubWave.rate * newDifficulty / 100);

        return new SubWave(
            toBuff,
            addCount,
            Random.Range(lastSubWave.rate - spawnRatePercentage, lastSubWave.rate + spawnRatePercentage)
            );
    }

    /// <summary>
    /// Returns an enemy prefab of a target tier
    /// </summary>
    /// <param name="tier">The enemy with the given tier to get</param>
    private GameObject GetRandomEnemyByTier(int tier)
    {
        List<GameObject> enemies = new List<GameObject>(availableEnemies);

        List<GameObject> ofTier = new List<GameObject>();

        // get the enemies of target tier
        enemies.ForEach(e =>
        {
            if (e.GetComponent<Enemy>().tier == tier) ofTier.Add(e);
        });

        // randomly return an enemy with the matching tier
        return ofTier[Random.Range(0, ofTier.Count)];
    }

    /// <summary>
    /// Gets the available enemies to spawn
    /// </summary>
    /// <returns></returns>
    //private HashSet<GameObject> GetAvailableEnemies()
    //{
    //    HashSet<GameObject> ret = new HashSet<GameObject>();

    //    foreach (var wave in waves)
    //    {
    //        foreach (var subwave in wave.subWaves)
    //        {
    //            ret.Add(subwave.enemy);
    //        }
    //    }

    //    return ret;
    //}
}
