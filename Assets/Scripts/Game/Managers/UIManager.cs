using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject coinPopupPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Updates the text score
    /// </summary>
    /// <param name="score">The score text that will update the gui</param>
    public void UpdateTextScore(int score)
    {
        GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    /// <summary>
    /// Updates the text balance
    /// </summary>
    /// <param name="balance">The balance that will update the gui</param>
    public void UpdateTextBalance(int balance)
    {
        GameObject.FindGameObjectWithTag("CoinBalance").GetComponent<TextMeshProUGUI>().text = balance.ToString();
    }
}
