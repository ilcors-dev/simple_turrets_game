using TMPro;
using UnityEngine;

public class CoinPopup : MonoBehaviour
{
    /// <summary>
    /// The movement speed of the text info damage on Y axis
    /// </summary>
    private float moveYSpeed;

    /// <summary>
    /// When 0 destroys the damage popup
    /// </summary>
    private float deathTimer;

    /// <summary>
    /// The popup text color
    /// </summary>
    private Color textColor;

    /// <summary>
    /// The text component
    /// </summary>
    private TextMeshPro textMesh;

    /// <summary>
    /// Creates a coin popup in a given position with a given set text
    /// </summary>
    /// <param name="position">The position the damage text will spawn</param>
    /// <param name="coinValue">The coin value to show in text</param>
    /// <returns>CoinPopup</returns>
    public static CoinPopup Create(int coinValue)
    {
        // get the coin balance text
        Vector3 coinTextPos = Camera.main.ScreenToWorldPoint(GameObject.FindGameObjectWithTag("CoinBalance").transform.position);

        // spawn the coin near the coin balance text
        Vector3 toSpawn =  new Vector3(coinTextPos.x - .35f, coinTextPos.y, 0);

        Transform coinPopupTransform = Instantiate(UIManager.Instance.coinPopupPrefab, Random.insideUnitSphere * .1f + toSpawn, Quaternion.identity).transform;

        CoinPopup coinPopup = coinPopupTransform.GetComponent<CoinPopup>();
        coinPopup.Setup(coinValue);

        return coinPopup;
    }

    // Start is called before the first frame update
    void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        // move up
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
        {
            // fade the text
            textColor.a -= 3f * Time.deltaTime;
            textMesh.color = textColor;

            // alpha channel 0, gameobject is invisible.. let's destroy it
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets up the damage popup
    /// </summary>
    /// <param name="coinValue">The coin value to show in text</param>
    public void Setup(int coinValue)
    {
        textMesh.SetText("+" + coinValue.ToString());

        //textMesh.faceColor = new Color32(0, 0, 0, 255);

        // movement speed on y axis of the popup
        moveYSpeed = .7f;

        textColor = textMesh.color;
        deathTimer = .5f;// init death timer of popup
    }
}
