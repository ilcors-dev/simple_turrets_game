using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // bullet speed
    public float speed;

    // the final bullet speed between the bullet and the target
    float finalSpeed;

    // is target locked?
    // if so the bullet won't update its target enemy
    public bool lockedTarget { set; get; }

    public GameObject closest { set; get; }
    void Start()
    {
        finalSpeed = speed * Time.deltaTime;
    }

    void Update()
    {
        // if target is not locked, find the nearest enemy
        //if (lockedTarget == false)
        //    closest = FindClosestEnemy();

        // if the target was found move the bullet towards the enemy
        if (closest != null)
        {
            lockedTarget = true;
            //transform.LookAt(closest.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, closest.transform.position, finalSpeed);
        }
        else Destroy(gameObject);
    }

    /**
     * <summary>
     * Finds the closests enemy gameobject
     * </summary>
     */
    GameObject FindClosestEnemy()
    {
        // get all enemies
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        // to return gameobject
        GameObject closest = null;

        // init distance between bullet and enemy
        float distance = Mathf.Infinity;

        foreach (GameObject e in enemyList)
        {
            // calculate the difference between the bullet and the enemy
            Vector3 diff = e.transform.position - gameObject.transform.position;

            // get the squared length of this vector
            float currentDistance = diff.sqrMagnitude;

            // if the current distance is less than the one calculated before on the previous enemy
            if (currentDistance < distance)
            {
                // assign new closest
                closest = e;
                // update distance with the current calculated one
                distance = currentDistance;
            }
        }

        return closest;
    }

    public void SetTarget(GameObject target)
    {
        closest = target;
        lockedTarget = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("hit: " + collision);
        Destroy(gameObject);
    }
}
