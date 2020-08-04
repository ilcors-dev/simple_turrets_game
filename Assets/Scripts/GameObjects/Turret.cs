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
    GameObject lockedTarget;

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
        if (Input.touchCount != 0)
        {
            EnableDisableRange();
        }
        if (Input.GetMouseButtonDown(0))
        {
            EnableDisableRange();
        }

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

    IEnumerator EnableDisableRange()
    {
        SpriteRenderer rangeSprite = range.GetComponent<SpriteRenderer>();
        if (rangeSprite.enabled)
            rangeSprite.enabled = false;
        else
            rangeSprite.enabled = true;
        yield return new WaitForSeconds(1.5f);
    }

    /// <summary>
    /// Shoots a bullet to a target enemy with a certain firerate.
    /// </summary>
    /// <param name="enemy">GameObject</param>
    /// <returns></returns>
    private IEnumerator Shoot(GameObject enemy)
    {
        while (lockedTarget != null && lockedTarget.Equals(enemy))
        {

            GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
            // get script
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            //bulletScript.SetTarget(enemy);
            bulletScript.lockedTarget = true;
            bulletScript.closest = enemy;
            bulletScript.damage = damage;// set bullet damage based on turret

            bullet.SetActive(true);
            Vector3 animationSpawn = gameObject.transform.position;

            GameObject shootExplosion = Instantiate(shootAnimation, shootAnimation.transform.position, Quaternion.identity);
            shootExplosion.SetActive(true);

            ParticleSystem animation = shootExplosion.GetComponentInChildren<ParticleSystem>();

            float animationDuration = animation.main.duration + animation.main.startLifetime.constant;

            shootExplosion.GetComponentInChildren<ParticleSystem>().Play();

            Destroy(shootExplosion, animationDuration);

            if (lockedTarget == null)// stop coroutine
                yield break;


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
        //Debug.Log(collision.tag);
        // if the collision is an enemy then shoot
        if (!collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Turret"))
        {
            //Debug.Log(collision.gameObject);

            //inRange.Add(collision.gameObject);
            if (lockedTarget == null)
            {
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
        if (lockedTarget.Equals(collision.gameObject))
            lockedTarget = null;
    }
}
