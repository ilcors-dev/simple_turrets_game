using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySilencer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SilenceTurret", 5f);
    }

    /// <summary>
    /// Picks a random turret and silences it
    /// </summary>
    private void SilenceTurret()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");

        turrets[Random.Range(0, turrets.Length - 1)].GetComponent<Turret>().isSilenced = true;
    }
}
