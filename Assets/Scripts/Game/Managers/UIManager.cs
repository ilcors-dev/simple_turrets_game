using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public GameObject coinPopupPrefab;

    [SerializeField]
    ///<summary>The coin balance ui component</summary>
    private TextMeshProUGUI coinBalanceUI;

    [SerializeField]
    ///<summary>The lives left ui component</summary>
    private TextMeshProUGUI livesLeftUI;

    [SerializeField]
    ///<summary>The death ui menu component</summary>
    private GameObject deathMenuUI;

    [SerializeField]
    ///<summary>The current round</summary>
    private TextMeshProUGUI roundIndexUI;

    [SerializeField]
    ///<summary>The next wave time countdown ui component</summary>
    private TextMeshProUGUI nextWaveTimerUI;

    // Start is called before the first frame update
    void Start()
    {
        SetLivesText();
        SetRoundIndexText();
    }

    /// <summary>
    /// Updates the overall cash earned
    /// </summary>
    /// <param name="score">The overall cash text that will update the gui</param>
    public void UpdateOverallCash(int score)
    {
        //GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    /// <summary>
    /// Updates the text balance
    /// </summary>
    /// <param name="balance">The balance that will update the gui</param>
    public void UpdateTextBalance(int balance)
    {
        coinBalanceUI.text = balance.ToString();
    }

    /// <summary>
    /// Shows the death menu
    /// </summary>
    public void ShowDeathUI()
    {
        deathMenuUI.SetActive(true);

        deathMenuUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Waves survived: " + GameManager.Instance.round);
        deathMenuUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("Cash earned: blabla");
    }
    
    /// <summary>
    /// Sets how many lives are left for the current round
    /// </summary>
    public void SetLivesText()
    {
        livesLeftUI.SetText("Lives left: " + ((GameManager.Instance.livesLeft < 0) ? 0 : GameManager.Instance.livesLeft));
    }

    /// <summary>
    /// Sets the countdown text for how much time is left before the next wave spawn
    /// </summary>
    public void SetNextWaveCountdownText(float timer)
    {
        string toDisplay = "Next wave: " + string.Format("{0:00.00}", timer);
        nextWaveTimerUI.SetText(toDisplay);
    }

    /// <summary>
    /// Sets the current wave / round of the game
    /// </summary>
    public void SetRoundIndexText()
    {
        roundIndexUI.SetText("Wave: " + GameManager.Instance.round.ToString());
    }

    /// <summary>
    /// Reloads the level
    /// </summary>
    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
