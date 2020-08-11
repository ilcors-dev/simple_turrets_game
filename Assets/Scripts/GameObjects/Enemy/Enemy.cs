using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Rigidbody2d reference
    /// </summary>
    private Rigidbody2D body;

    /// <summary>
    /// Health of the enemy, seriazable field
    /// </summary>
    public int health;

    /// <summary>
    /// Is enemy death?
    /// </summary>
    private bool death;

    /// <summary>
    /// The enemy helth text reference
    /// </summary>
    TextMeshProUGUI healthText;

    /// <summary>
    /// The enemy worth value
    /// </summary>
    [Tooltip("When killed the coins will be added based on the enemie's coin value")]
    public int worthCoinValue;

    // Start is called before the first frame update
    void Start()
    {
        // init componenents
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
        // if the collision is the death wall, destroy enemy
        // TODO: decrement player live / end game
        if (collision.CompareTag("DeathWall"))
        {
            GameManager.Instance.DecrementLives();
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Deals damage to the enemy.
    /// </summary>
    /// <param name="dmg"></param>
    public void DealDamage(int dmg)
    {
        // decrement enemy health
        health -= dmg;
        // update health text
        healthText.SetText(health.ToString());

        // if health is <= 0 the enemy is death
        // start death animation
        if (health <= 0)
        {
            ShowDeathAnimation();

            CoinPopup.Create(worthCoinValue);

            // destroy the actual enemy,
            // this does not cause any problem because the death animation we created
            // is not a child of this gameobject but instead it got instantiate in the place where
            // the gameobject health went below 0
            Destroy(gameObject);

            GameManager.Instance.UpdateBalance(worthCoinValue);
            //GameManager.Instance.UpdateScore();
        }

    }

    /// <summary>
    /// Shows the death animation of the turret
    /// </summary>
    private void ShowDeathAnimation()
    {
        // get animation
        GameObject animation = transform.GetChild(2).gameObject;

        // copy the death animation of the object
        GameObject deathAnimation = Instantiate(animation, transform.position, Quaternion.identity);
        deathAnimation.SetActive(true);

        // get its particle system which actually holds the animation stuff
        ParticleSystem pp = deathAnimation.GetComponent<ParticleSystem>();

        // calculate its duration to later destroy it after this amount of time
        float animationDuration = pp.main.duration + pp.main.startLifetime.constant;

        // start death animation
        deathAnimation.GetComponent<ParticleSystem>().Play();

        // destroy animation after it's finished
        Destroy(deathAnimation, animationDuration);
    }
}
