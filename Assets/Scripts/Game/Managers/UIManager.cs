using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
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
}
