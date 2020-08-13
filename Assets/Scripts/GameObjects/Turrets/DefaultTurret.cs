using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTurret : Turret
{
    /// <summary>
    /// The bullet that gets instantiated
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// The animation shown when shooting
    /// </summary>
    public GameObject shootAnimation;

    [SerializeField]
    private Transform shootPosition;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("AcquireTarget", 0f, 0.3f);
        //inRange = new HashSet<GameObject>();
        range = transform.GetChild(0).GetComponent<SpriteRenderer>();
        shootAnimation = gameObject.transform.GetChild(2).gameObject;

        // draw turret range
        DrawRange();

        // get turret infos gui
        turretInfosUI = transform.GetChild(3).gameObject;

        // get audio source
        audio = GetComponent<AudioSource>();

        turretLevel = 1;

        isSilenced = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if no target is acquired, return
        if (lockedTarget == null) return;
        if (isSilenced) return;

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

        // update turret infos if open each frame
        if (turretInfosUI.activeSelf)
        {
            UpdateInfos();
            UpdateUpgradePreviewInfos();
        }
    }

    /// <summary>
    /// Shoots a bullet to a target enemy with a certain firerate.
    /// </summary>
    /// <param name="enemy">GameObject</param>
    /// <returns></returns>
    public override void Shoot(Transform enemy)
    {
        audio.Play();

        if (IsCritDamage())
        {
            // create new bullet with critical applied
            Bullet.Create(bulletPrefab, new Vector3(shootPosition.position.x, shootPosition.position.y, enemy.position.z), enemy.transform, this, damage + IncreaseByPercentageInt(damage, critIncrease), true);
        }
        else
            // create new bullet
            Bullet.Create(bulletPrefab, new Vector3(shootPosition.position.x, shootPosition.position.y, enemy.position.z), enemy.transform, this, damage, false);

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
}
