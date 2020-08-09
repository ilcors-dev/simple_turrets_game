﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret specs")]
    /// <summary>
    /// turret damage
    /// </summary>
    [SerializeField]
    private int damage = 1;

    /// <summary>
    /// firerate of the turret in seconds
    /// </summary>
    [SerializeField]
    private float fireRate;

    /// <summary>
    /// Counter backwards, when 0 is reached then the turret will fire if a target is locked
    /// </summary>
    private float fireCountdown = 0f;

    /// <summary>
    /// range of the turret
    /// </summary>
    [SerializeField]
    private float fireRange;

    /// <summary>
    /// the range of the turret
    /// </summary>
    GameObject range;

    [Header("Turret prefabs")]

    /// <summary>
    /// The bullet that gets instantiated
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// The turret locked target
    /// </summary>
    private Transform lockedTarget { get; set; }

    /// <summary>
    /// The animation shown when shooting
    /// </summary>
    public GameObject shootAnimation;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("AcquireTarget", 0f, 0.2f);
        //inRange = new HashSet<GameObject>();
        range = transform.GetChild(0).gameObject;
        shootAnimation = gameObject.transform.GetChild(2).gameObject;
        DrawRange();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // if no target is acquired, return
        if (lockedTarget == null)
        {
            return;
        }

        // always rotate towards target if the enemy is locked
        RotateTorwardsTarget();

        // if the countdown is <= 0 it's time to shoot
        if (fireCountdown <= 0f)
        {
            Shoot(lockedTarget);

            fireCountdown = 1f / fireRate;
        }

        // decrement the fireCountdown
        fireCountdown -= Time.deltaTime;
    }

    /// <summary>
    /// Acquires the nearest enemy and locks it.
    /// </summary>
    private void AcquireTarget()
    {
        // get all the enemies in the field
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // temporary nearest enemy
        GameObject nearestEnemy = null;

        // shortest distance to enemy
        float shortestDistance = Mathf.Infinity;

        // the turret position
        Vector3 currentPos = transform.position;

        foreach (GameObject enemy in enemies)
        {
            // this transform distance to enemy
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, currentPos);

            // if the calculated distance is less than the one calculated before
            // it means that the current enemy is the nearest one
            if (distanceToEnemy < shortestDistance)
            {
                nearestEnemy = enemy;
                shortestDistance = distanceToEnemy;
            }
        }
        // if the nearest enemy is not null and its distance is in the turret range then we can lock the target
        if (nearestEnemy != null && shortestDistance <= fireRange)
            lockedTarget = nearestEnemy.transform;
        else lockedTarget = null;// nearest enemy is not in range or no enemies at all found
    }

    private void OnMouseDown()
    {
        EnableDisableRange();
    }

    /// <summary>
    /// Enables/disables the turret range indicator
    /// </summary>
    void EnableDisableRange()
    {
        SpriteRenderer rangeSprite = range.GetComponent<SpriteRenderer>();
        if (rangeSprite.enabled)
            rangeSprite.enabled = false;
        else
            rangeSprite.enabled = true;
    }

    /// <summary>
    /// Shoots a bullet to a target enemy with a certain firerate.
    /// </summary>
    /// <param name="enemy">GameObject</param>
    /// <returns></returns>
    public void Shoot(Transform enemy)
    {
        audio.Play();
        // spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(shootAnimation.transform.position.x, shootAnimation.transform.position.y, enemy.position.z), Quaternion.identity);
        // get script
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        // assign target to the bullet
        bulletScript.lockedTarget = true;
        bulletScript.target = enemy;
        bulletScript.damage = damage;// set bullet damage based on turret specs

        bullet.SetActive(true);// show bullet

        // show shoot animation of the turret
        GameObject shootExplosion = Instantiate(shootAnimation, new Vector3(shootAnimation.transform.position.x, shootAnimation.transform.position.y, -5f), Quaternion.identity);
        //shootExplosion.transform.LookAt(enemy.transform);
        shootExplosion.SetActive(true);

        // get its particle system which actually holds the particle stuff
        ParticleSystem animation = shootExplosion.GetComponentInChildren<ParticleSystem>();

        // calculate the animation duration
        float animationDuration = animation.main.duration + animation.main.startLifetime.constant;

        // start the animation
        animation.Play();

        // destroy it after the animation finished
        Destroy(shootExplosion, animationDuration);
    }

    /// <summary>
    /// Rotates the turret towards the locked target.
    /// </summary>
    private void RotateTorwardsTarget()
    {
        ////find the vector pointing from our position to the target
        Vector3 targetDirection = lockedTarget.transform.position - transform.position;

        //// Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, targetDirection, Color.red);

        var offset = 90f;
        Vector2 direction = (Vector2)lockedTarget.transform.position - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle - offset));
    }

    /// <summary>
    /// Draws the range of the turret
    /// </summary>
    private void DrawRange()
    {
        // dunno why, but the actual radius of the turret seems to be the 2/3 of the actual set radius
        // the turret will also detect an enemy in this range and not in the actual set one..
        float radius = (fireRange * 10 * 2) / 3;
        range.transform.localScale = new Vector3(radius, radius, radius);
    }

    /// <summary>
    /// Shows the range of the turret
    /// </summary>
    private void OnDrawGizmos()
    {
        // dunno why, but the actual radius of the turret seems to be the 2/3 of the actual set radius
        // the turret will also detect an enemy in this range and not in the actual set one..
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange * 2 / 3);
    }
}
