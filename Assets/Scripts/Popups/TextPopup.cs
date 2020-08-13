using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
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
    /// Creates a text popup in a given position with a given set text
    /// </summary>
    /// <param name="position">The position the damage text will spawn</param>
    /// <param name="text">The text to display</param>
    /// <returns></returns>
    public static TextPopup Create(Vector3 position, string text, Color32 color)
    {
        Transform textTransform = Instantiate(UIManager.Instance.textPopupPrefab, Random.insideUnitSphere * .1f + position, Quaternion.identity).transform;

        TextPopup textPopup = textTransform.gameObject.GetComponent<TextPopup>();
        textPopup.Setup(text, color);

        return textPopup;
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
    /// Sets up the text popup
    /// </summary>
    /// <param name="text">The text to set</param>
    public void Setup(string text, Color32 color)
    {
        textMesh.SetText(text.ToString());

        // movement speed on y axis of the popup
        moveYSpeed = 1.5f;

        // the text color
        textMesh.faceColor = color;

        // init death timer of popup
        deathTimer = .7f;
    }
}
