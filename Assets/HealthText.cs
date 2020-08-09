using UnityEngine;

public class HealthText : MonoBehaviour
{
    GameObject parent;

    RectTransform txt;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        txt = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.anchoredPosition = WorldToCanvasPoint(parent.transform.position);
    }

    public Vector2 WorldToCanvasPoint(Vector3 a_position)
    {
        Rect screen = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect;
        Vector2 viewport = Camera.main.WorldToViewportPoint(a_position);
        return Vector2.right * (viewport.x - 0.5f) * screen.width + Vector2.up * (viewport.y - 0.5f) * screen.height;
    }
}
