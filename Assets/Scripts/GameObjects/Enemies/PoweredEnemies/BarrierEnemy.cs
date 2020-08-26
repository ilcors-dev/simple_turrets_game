using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarrierEnemy : Enemy
{
    [Header("The barrier specs")]
    /// <summary>
    /// The barrier parent containing the barrier sprites
    /// </summary>
    [SerializeField]
    private GameObject barrier;

    /// <summary>
    /// The barrier health, once 0 the barrier will get destroyed
    /// </summary>
    [SerializeField]
    public float barrierHealth;

    /// <summary>
    /// The barrier health text
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI barrierText;

    /// <summary>
    /// The barrier explosion
    /// </summary>
    [SerializeField]
    private GameObject barrierExplosion;

    private void Start()
    {
        barrierText.SetText(barrierHealth.ToString());
    }

    public void DamageBarrier(int dmg)
    {
        barrierHealth -= dmg;

        if (barrierHealth <= 0)
        {
            DestroyBarrier();
        }

        // update barrier health text
        barrierText.SetText(barrierHealth.ToString());
    }

    private void DestroyBarrier()
    {
        GameObject explosion = Instantiate(barrierExplosion.gameObject, transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();

        barrier.SetActive(false);

        gameObject.tag = "Enemy";

        healthText.transform.parent.gameObject.SetActive(true);

        healthText.SetText(health.ToString());
    }
}
