using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// Creates a new bullet gameobject and initializes his attributes.
    /// </summary>
    /// <param name="bulletPrefab">The bullet prefab to spawn</param>
    /// <param name="position">The position where the bullet should spawn</param>
    /// <param name="target">The target of the bullet</param>
    /// <param name="parent">The parent turret script of the bullet</param>
    /// <param name="damage">The damage the bullet will deal to the enemy</param>
    /// <param name="isCriticalDamage">If the bullet will deal critical damage</param>
    public static void Create(GameObject bulletPrefab, Vector3 position, Transform target, Turret parent, int damage, bool isCriticalDamage)
    {
        // spawn bullet
        Bullet b = Instantiate(bulletPrefab, position, Quaternion.identity).GetComponent<Bullet>();

        // setup bullet
        b.Setup(target, parent, damage, isCriticalDamage);
    }

    /// <summary>
    /// Initializes the bullet attributes.
    /// </summary>
    /// <param name="target">The target of the bullet</param>
    /// <param name="parent">The parent turret script of the bullet</param>
    /// <param name="damage">The damage the bullet will deal to the enemy</param>
    /// <param name="isCriticalDamage">If the bullet will deal critical damage</param>
    protected void Setup(Transform target, Turret parent, int damage, bool isCriticalDamage)
    {
        // assign target to the bullet
        lockedTarget = true;

        // the target enemy
        this.target = target;

        // set bullet damage based on turret specs
        this.damage = damage;

        // show bullet
        gameObject.SetActive(true);

        // turret who shoot the bullet
        shootBy = parent;

        // will the bullet deal critical damage?
        this.isCriticalDamage = isCriticalDamage;
    }

    /// <summary>
    /// bullet speed
    /// </summary>
    public float speed;

    /// <summary>
    ///      the final bullet speed between the bullet and the target
    /// </summary>
    protected float finalSpeed;

    /// <summary>
    /// The damage of the bullet
    /// </summary>
    protected int damage { set; get; }

    // is target locked?
    // if so the bullet won't update its target enemy
    protected bool lockedTarget { set; get; }

    /// <summary>
    /// The target enemy
    /// </summary>
    protected Transform target { set; get; }

    /// <summary>
    /// Holds the reference of the turret that fired the bullet
    /// </summary>
    protected Turret shootBy;

    /// <summary>
    /// Will the bullet damage be critical?
    /// </summary>
    protected bool isCriticalDamage;

    void Start()
    {
        finalSpeed = speed * Time.deltaTime;
    }

    void Update()
    {

        // if the target was found move the bullet towards the enemy
        if (target != null)
        {
            lockedTarget = true;
            //transform.LookAt(target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// Sets the target gameobject which the bullet will follow and target
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        this.target = target;
        lockedTarget = true;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // if bullet hits the enemy, destroy the bullet and deal damage to enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // create damage info
            DamagePopup.Create(collision.transform.position, damage, isCriticalDamage);

            // if the bullet will kill the enemy, increment the turret killed enemies attribute
            if (enemy.Health - damage <= 0)
            {
                shootBy.totalKilled++;
                shootBy.UpdateInfos();
            }

            enemy.DealDamage(damage);
            Destroy(gameObject);
        }
        // if bullet hits the barrier of an enemy, destroy the bullet and deal damage to the barrier enemy
        if (collision.gameObject.CompareTag("Barrier"))
        {
            BarrierEnemy enemy = collision.gameObject.GetComponent<BarrierEnemy>();

            // create damage info
            DamagePopup.Create(collision.transform.position, damage, isCriticalDamage);

            enemy.DamageBarrier(damage);
            Destroy(gameObject);
        }
    }
}
