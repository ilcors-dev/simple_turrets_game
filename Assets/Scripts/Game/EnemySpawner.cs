using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    public GameObject toSpawn;
    public float spawnEvery;

    public Vector3 spawnPosition { get;  set; }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartEnemySpawn()
    {
        StartCoroutine(DelaySpawn());
    }

    private IEnumerator DelaySpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnEvery);
            GameObject spawned = Instantiate(toSpawn, spawnPosition, Quaternion.identity);
            spawned.SetActive(true);
        }
    }
}
