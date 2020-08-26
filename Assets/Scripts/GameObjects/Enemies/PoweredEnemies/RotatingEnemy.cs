using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnemy : Enemy
{
    [Header("Rotating enemy attributes")]
    /// <summary>
    /// The enemy skin
    /// </summary>
    [SerializeField]
    private GameObject skin;

    /// <summary>
    /// The enemy total health
    /// </summary>
    private int totalHealth;

    /// <summary>
    /// The health value where the enemy will use its power-ups
    /// </summary>
    private int percentageBelowHealth;

    /// <summary>
    /// The range from the enemy where the tiles will get invalidated
    /// </summary>
    [SerializeField]
    private float invalidateTileRange;

    [SerializeField]
    private float repeatInvalidation;

    [Header("Rotating enemy prefabs")]
    [SerializeField]
    private GameObject invalidateTileProjectile;

    void Start()
    {
        totalHealth = health;

        percentageBelowHealth = (int)(totalHealth * .60f);

        // init componenents
        body = GetComponent<Rigidbody2D>();
        healthText.SetText(health.ToString());

        InvokeRepeating("InvalidateTile", 5f, repeatInvalidation);
    }


    bool invalidating = false;
    void Update()
    {
        for (int i = 0; i < skin.transform.childCount; i++)
        {
            Transform child = skin.transform.GetChild(i);
            if (health <= percentageBelowHealth)
            {
                child.RotateAround(transform.position, new Vector3(0, 0, 500 * Time.deltaTime), 1f);
            }
            else child.RotateAround(transform.position, new Vector3(0, 0, 1f * Time.deltaTime), 1f);
        }
        if (!invalidating && health < percentageBelowHealth)
        {
            invalidating = true;
            InvokeRepeating("InvalidateTile", 0f, repeatInvalidation);
        }
    }

    void InvalidateTile()
    {
        List<GameObject> toInvalidate = GetTilesInRange();

        Transform target = toInvalidate[Random.Range(0, toInvalidate.Count)].transform;

        //target.GetComponent<TileScript>().InvalidateTile();
        ShootProjectile(target);
    }

    List<GameObject> GetTilesInRange()
    {
        List<GameObject> tiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("MapTile"));

        List<GameObject> inRange = new List<GameObject>();

        // the turret position
        Vector3 currentPos = transform.position;

        foreach (GameObject tile in tiles)
        {
            // this transform distance to enemy
            float distanceToTile = Vector3.Distance(tile.transform.position, currentPos);

            // if the calculated distance is less than the one calculated before
            if (distanceToTile < invalidateTileRange && !tile.GetComponent<TileScript>().occupiedTile)
                inRange.Add(tile);
        }

        return inRange;
    }

    void ShootProjectile(Transform target)
    {
        GameObject projectile = Instantiate(invalidateTileProjectile, transform.position, Quaternion.identity);

        projectile.GetComponent<EnemyProjectile>().SetTarget(target);
    }
}
