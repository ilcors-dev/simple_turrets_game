using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
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
    /// Creates a damage popup in a given position with a given set text
    /// </summary>
    /// <param name="position">The position the damage text will spawn</param>
    /// <param name="damageDealt">The dealt damage shown in text</param>
    /// <param name="isCriticalDamage">If the damage dealt is critical the popup will be different</param>
    /// <returns>DamagePopup</returns>
    public static DamagePopup Create(Vector3 position, int damageDealt, bool isCriticalDamage)
    {
        Transform damagePopupTransform = Instantiate(EnemyManager.Instance.damagePopup, Random.insideUnitSphere * .1f + position, Quaternion.identity).transform;

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageDealt, isCriticalDamage);

        return damagePopup;
    }

    private TextMeshPro textMesh;
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
        if(deathTimer <= 0)
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
    /// <param name="damageDealt">The damage dealt to set as text</param>
    /// <param name="isCriticalDamage">If the damage dealt is critical the popup will be different</param>
    public void Setup(int damageDealt, bool isCriticalDamage)
    {
        textMesh.SetText(damageDealt.ToString());

        // movement speed on y axis of the popup
        moveYSpeed = 2f;

        if (!isCriticalDamage)// not critical
        {
            textColor = textMesh.color;
        }
        else// critical damage
        {
            textColor = new Color32(191, 37, 37, 255);
            textMesh.faceColor = textColor;// change color if crit damage
            textMesh.fontSize = 2f;// make bigger text on crit damage
        }

        deathTimer = 1f;// init death timer of popup
    }
}
