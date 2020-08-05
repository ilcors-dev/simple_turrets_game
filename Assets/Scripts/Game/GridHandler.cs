using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    /// <summary>
    /// The object to set into world
    /// </summary>
    public GameObject finalObject;

    private Vector2 touchPos;

    public LayerMask allTilesLayer;

    private Grid grid;

    private void Update()
    {
        //touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(touchPos.x), Mathf.Round(touchPos.y));

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
            RaycastHit2D rayHit = Physics2D.Raycast(mouseRay, Vector2.zero, Mathf.Infinity, allTilesLayer);

            if (rayHit.collider == null)
            {
                Instantiate(finalObject, transform.position, Quaternion.identity);
            }
        }
    }
}
