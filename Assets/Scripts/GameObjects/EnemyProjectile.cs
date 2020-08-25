using UnityEngine;
using System.Collections;

public class EnemyProjectile : Bullet
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // if the target was found move the bullet towards the enemy
        if (target != null)
        {
            lockedTarget = true;
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
            transform.RotateAround(transform.position, new Vector3(0, 0, 3f * Time.deltaTime), 1f);
        }
        else Destroy(gameObject);
    }
    
    new void OnCollisionEnter2D(Collision2D collision)
    {
        // if bullet hits the target tile, deactivate the tile and destroy the bullet
        if (collision.gameObject.CompareTag("MapTile") && target.Equals(collision.transform))
        {
            TileScript tile = collision.gameObject.GetComponent<TileScript>();
            
            tile.InvalidateTile();

            Destroy(gameObject);
        }
    }

    public Transform GetTarget()
    {
        return target;
    }
}
