using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
/// <summary>
/// bullet speed
/// </summary>
    public float speed;

    /// <summary>
    ///      the final bullet speed between the bullet and the target
    /// </summary>
    float finalSpeed;

    /// <summary>
    /// The damage of the bullet
    /// </summary>
    public int damage {set; get;}

    // is target locked?
    // if so the bullet won't update its target enemy
    public bool lockedTarget { set; get; }

    /// <summary>
    /// The target enemy
    /// </summary>
    public Transform target { set; get; }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if bullet hits the enemy, destroy the bullet and deal damage to enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DamagePopup.Create(collision.transform.position, damage, true);
            collision.gameObject.GetComponent<Enemy>().DealDamage(damage);
            Destroy(gameObject);
        }
    }
}
