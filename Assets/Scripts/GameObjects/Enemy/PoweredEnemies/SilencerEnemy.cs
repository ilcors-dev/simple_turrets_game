using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilencerEnemy : Enemy
{
    [Header("Silence specs")]
    /// <summary>
    /// How often the turret will silence a turret
    /// </summary>
    [SerializeField]
    private float silenceEvery;

    /// <summary>
    /// The silenced turret by the enemy
    /// </summary>
    List<Turret> silencedTurrets;

    /// <summary>
    /// The sprite to append on the silenced turrets
    /// </summary>
    [SerializeField]
    private GameObject silencedPrefab;

    // Start is called before the first frame update
    new void Start()
    {
        // init componenents
        body = GetComponent<Rigidbody2D>();
        healthText.SetText(health.ToString());

        silencedTurrets = new List<Turret>();

        InvokeRepeating("SilenceTurret", 5f, silenceEvery);
    }

    /// <summary>
    /// Picks a random turret and silences it
    /// </summary>
    private void SilenceTurret()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        
        Turret target;

        // silence random turret
        do {
            target = turrets[Random.Range(0, turrets.Length)].GetComponent<Turret>();
        }
        while (target.isSilenced);// do not get already silenced turrets

        if (!target.isSilenced)
        {
            target.isSilenced = true;
            TextPopup.Create(target.gameObject.transform.position, "SILENCED", new Color32(0, 0, 0, 255));

            SpriteRenderer targetSize = target.GetComponent<SpriteRenderer>();

            // spawn lock with the turret and set its parent to the turrett
            Instantiate(silencedPrefab, new Vector3(target.transform.position.x + (targetSize.size.x / 5), target.transform.position.y + (targetSize.size.y / 5), -1f), Quaternion.identity)
                .transform.SetParent(target.transform);
            silencedTurrets.Add(target);
        }
    }

    private void OnDestroy()
    {
        // un-silence the silenced turrets
        silencedTurrets.ForEach(t =>
        {
            t.isSilenced = false;

            // remove lock icon
            for (int i = 0; i < t.transform.childCount; i++)
            {
                Transform tmp = t.transform.GetChild(i);
                if (tmp.CompareTag("SilencedIcon")) 
                    Destroy(tmp.gameObject);
            }
        });
    }
}
