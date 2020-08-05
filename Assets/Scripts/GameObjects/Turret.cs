using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    /// <summary>
    /// turret damage
    /// </summary>
    public int damage = 1;

    /// <summary>
    /// firerate of the turret in seconds
    /// </summary>
    public float fireRate;

    /// <summary>
    /// range of the turret
    /// </summary>
    public float fireRange = 1f;

    /// <summary>
    /// The bullet that gets instantiated
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// the range of the turret
    /// </summary>
    GameObject range;

    // enemies in range
    //public HashSet<GameObject> inRange;

    // shoot only at the locked target
    public GameObject lockedTarget { get; set; }

    public GameObject shootAnimation;

    // Start is called before the first frame update
    void Start()
    {
        //inRange = new HashSet<GameObject>();
        range = transform.GetChild(0).gameObject;
        shootAnimation = gameObject.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockedTarget != null)
        {
            ////find the vector pointing from our position to the target
            Vector3 targetDirection = lockedTarget.transform.position - transform.position;

            //// Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, targetDirection, Color.red);

            //transform.rotation = Quaternion.LookRotation(targetDirection);

            var offset = 90f;
            Vector2 direction = (Vector2)lockedTarget.transform.position - (Vector2)transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle - offset));
        }
    }

    //private void OnMouseDown()
    //{
    //    EnableDisableRange();
    //}

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
    public IEnumerator Shoot(GameObject enemy)
    {
        // shoot only if the turret has acquire a target
        // and only if the target equals to the locked one
        while (lockedTarget != null && lockedTarget.Equals(enemy))
        {
            // spawn bullet
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(shootAnimation.transform.position.x, shootAnimation.transform.position.y, enemy.transform.position.z), Quaternion.identity);
            // get script
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            
            // assign target to the bullet
            bulletScript.lockedTarget = true;
            bulletScript.closest = enemy;
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

            // if the locked target died or existed the range locked target will be set to null
            // stop this coroutine for optimization reasons
            if (lockedTarget == null)// stop coroutine
                yield break;

            // wait before shooting again
            // fireRate is a turret variable in seconds
            yield return new WaitForSeconds(fireRate);
        }
    }

    //void PrintInRange()
    //{
    //    foreach (GameObject o in inRange)
    //    {
    //        Debug.Log(o);
    //    }
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        // if the collision is an enemy then shoot
        if (!collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Turret"))
        {
            // if turret has no locked any enemy
            if (lockedTarget == null)
            {
                // acquire target
                lockedTarget = collision.gameObject;
                // shot only if enemy is locked
                StartCoroutine(Shoot(collision.gameObject));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log(inRange.Count);
        //if (inRange.Contains(collision.gameObject))
        //{
        //    //Debug.Log("Removed: " + collision.gameObject);
        //    inRange.Remove(collision.gameObject);
        //}

        // target got out of the range, remove target
        if (lockedTarget != null && lockedTarget.Equals(collision.gameObject))
            lockedTarget = null;
    }
}
