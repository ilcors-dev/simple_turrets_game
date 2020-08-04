using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public int damage = 1;
    public float fireRate = 1f;

    public float fireRange = 1f;

    public GameObject bulletPrefab;

    GameObject range;

    // enemies in range
    public HashSet<GameObject> inRange;

    // Start is called before the first frame update
    void Start()
    {
        inRange = new HashSet<GameObject>();
        range = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            SpriteRenderer rangeSprite = range.GetComponent<SpriteRenderer>();
            if (rangeSprite.enabled)
                rangeSprite.enabled = false;
            else
                rangeSprite.enabled = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            SpriteRenderer rangeSprite = range.GetComponent<SpriteRenderer>();
            if (rangeSprite.enabled)
                rangeSprite.enabled = false;
            else
                rangeSprite.enabled = true;
        }

        //Vector3 diff = transform.position - gameObject.transform.position;
        //if (transform.GetChild(0).GetComponent<CircleCollider2D>().radius<)
        //    Debug.Log("ss");
    }

    private IEnumerator Shoot(GameObject enemy)
    {
        while (inRange.Contains(enemy))
        {
            yield return new WaitForSeconds(fireRate);
            GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
            // get script
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            //bulletScript.SetTarget(enemy);
            bulletScript.lockedTarget = true;
            bulletScript.closest = enemy;

            bullet.SetActive(true);
        }
    }

    void PrintInRange()
    {
        foreach (GameObject o in inRange)
        {
            Debug.Log(o);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.tag);
        if (!collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Turret"))
        {
            Debug.Log(collision.gameObject);

            inRange.Add(collision.gameObject);

            StartCoroutine(Shoot(collision.gameObject));
            PrintInRange();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log(inRange.Count);
        if (inRange.Contains(collision.gameObject))
        {
            //Debug.Log("Removed: " + collision.gameObject);
            inRange.Remove(collision.gameObject);
        }
    }
}
