using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //[SerializeField]
    //private GameObject towerPrefab;

    /// <summary>
    /// The overall cash earned per wave
    /// Each wave has its own value, the more difficult the more value
    /// </summary>
    private int overallCash;

    /// <summary>
    /// Coins earned this round
    /// </summary>
    public int coins { get; set; }

    /// <summary>
    /// The current turret shown range.
    /// Used to show just one range per time
    /// </summary>
    public SpriteRenderer shownRange { get; set; }

    /// <summary>
    /// The current shown turret infos UI.
    /// Used to show just one turret infos UI per time
    /// </summary>
    public GameObject shownTurretInfos { get; set; }

    /// <summary>
    /// How many lives the user has left.
    /// When an enemy reaches the end this will get decreased.
    /// </summary>
    public int livesLeft;

    /// <summary>
    /// The current wave
    /// </summary>
    public int round;

    /// <summary>
    /// This will hold the turret when bought, on tap it will get instantiated on the scene
    /// </summary>
    public GameObject boughtTurret;

    private void Start()
    {
        UpdateBalance(150);
    }

    //public GameObject TowerPrefab
    //{
    //    get
    //    {
    //        return towerPrefab;
    //    }
    //}

    /// <summary>
    /// Updates the overall cash game currency and the score text
    /// </summary>
    public void UpdateOverallCash()
    {
        overallCash++;
        //UIManager.Instance.UpdateTextScore(overallCash);
    }

    /// <summary>
    /// Updates the balance and the balance text
    /// </summary>
    public void UpdateBalance(int coinValue)
    {
        coins += coinValue;
        UIManager.Instance.UpdateTextBalance(coins);
    }

    public void DecrementLives()
    {
        livesLeft--;
        UIManager.Instance.SetLivesText();
        if (livesLeft == 0)
        {
            EndGame();
        }
    }
    
    /// <summary>
    /// Ends the game and show the death UI
    /// </summary>
    private void EndGame()
    {
        Time.timeScale = 0;// freeze game and show death UI
        UIManager.Instance.ShowDeathUI();
    }
}
