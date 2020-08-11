using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject coinPopupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SetLivesText();
    }

    // Update is called once per frame
    void Update()
    {

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
        GameObject.FindGameObjectWithTag("CoinBalance").GetComponent<TextMeshProUGUI>().text = balance.ToString();
    }

    /// <summary>
    /// Shows the death menu
    /// </summary>
    public void ShowDeathUI()
    {
        GameObject deathUI = transform.GetChild(2).gameObject;
        deathUI.SetActive(true);

        deathUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Waves survived: blabla");
        deathUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("Cash earned: blabla");
    }
    
    /// <summary>
    /// Sets how many lives are left for the current round
    /// </summary>
    public void SetLivesText()
    {
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().SetText("Lives left: " + GameManager.Instance.livesLeft);
    }
}
