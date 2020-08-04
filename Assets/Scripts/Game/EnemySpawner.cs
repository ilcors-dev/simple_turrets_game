using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject toSpawn;
    public float spawnEvery;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelaySpawn());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator DelaySpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnEvery);
            GameObject spawned = Instantiate(toSpawn);
            spawned.SetActive(true);
        }
    }
}
