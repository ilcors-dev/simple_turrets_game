using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject towerPrefab;

    /// <summary>
    /// The player score
    /// </summary>
    private int score = 0;

    /// <summary>
    /// The current turret shown range.
    /// Used to show just one range per time.
    /// </summary>
    public SpriteRenderer shownRange { get; set; }

    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    public void UpdateScore()
    {
        score++;
        UIManager.Instance.UpdateTextScore(score);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
