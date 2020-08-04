using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D body;

    public int health;

    private bool death;

    TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        healthText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        healthText.SetText(health.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //body.velocity = new Vector2(body.velocity.x + 0.001f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathWall"))
            Destroy(gameObject);
    }

    public void DealDamage(int dmg)
    {
        health -= dmg;
        healthText.SetText(health.ToString());
        if (health <= 0)
        {
            GameObject animation = transform.GetChild(2).gameObject;

            animation.SetActive(true);

            GameObject deathAnimation = Instantiate(animation, transform.position, Quaternion.identity);
            deathAnimation.SetActive(true);
           

            ParticleSystem pp = deathAnimation.GetComponent<ParticleSystem>();

            float animationDuration = pp.main.duration + pp.main.startLifetime.constant;

            deathAnimation.GetComponent<ParticleSystem>().Play();

            Destroy(deathAnimation, animationDuration);

            Destroy(gameObject);
        }

    }
}
