using TMPro;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    /// <summary>
    /// Rigidbody2d reference
    /// </summary>
    protected Rigidbody2D body;

    /// <summary>
    /// Health of the enemy, seriazable field
    /// </summary>
    public int health;

    /// <summary>
    /// Is enemy death?
    /// </summary>
    protected bool death;

    /// <summary>
    /// The enemy helth text reference
    /// </summary>
    [SerializeField]
    protected TextMeshProUGUI healthText;

    /// <summary>
    /// The enemy death animation
    /// </summary>
    [SerializeField]
    protected GameObject deathAnimation;

    /// <summary>
    /// The enemy worth value
    /// </summary>
    [Tooltip("When killed the coins will be added based on the enemie's coin value")]
    public int worthCoinValue;

    /// <summary>
    /// The enemy lives worth value
    /// </summary>
    [Tooltip("How many lives the enemy decreases the round lives when it reaches the end of the path")]
    public int worthLiveValue;

    // Update is called once per frame
    void Update()
    {
        //body.velocity = new Vector2(body.velocity.x + 0.001f, 0);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // if the collision is the death wall, destroy enemy
        // TODO: decrement player live / end game
        if (collision.CompareTag("DeathWall"))
        {
            GameManager.Instance.DecrementLives(worthLiveValue);
            EnemySpawner.EnemiesAlive--;
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
            Die();

    }

    /// <summary>
    /// Does stuff when the enemy health is <= 0.
    /// Instantiates death effects and updates statistics.
    /// </summary>
    protected void Die()
    {
        ShowDeathAnimation();

        CoinPopup.Create(worthCoinValue);

        EnemySpawner.EnemiesAlive--;

        // destroy the actual enemy,
        // this does not cause any problem because the death animation we created
        // is not a child of this gameobject but instead it got instantiate in the place where
        // the gameobject health went below 0
        Destroy(gameObject);

        GameManager.Instance.UpdateBalance(worthCoinValue);
        //GameManager.Instance.UpdateScore();
    }

    /// <summary>
    /// Shows the death animation of the turret
    /// </summary>
    protected void ShowDeathAnimation()
    {
        // copy the death animation of the object
        GameObject deathAnimation = Instantiate(this.deathAnimation, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
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
