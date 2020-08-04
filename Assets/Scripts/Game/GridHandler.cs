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

    private void Update()
    {
        //touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(touchPos.x), Mathf.Round(touchPos.y));

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(finalObject, transform.position, Quaternion.identity);
        }
    }
}
