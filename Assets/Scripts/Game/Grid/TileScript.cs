using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

    private float TileXSize, TileYSize;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer rd = GetComponent<SpriteRenderer>();
        TileXSize = rd.size.x;
        TileYSize = rd.size.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Sets up the tile
    /// </summary>
    /// <param name="gridPos">The tile's grid position</param>
    /// <param name="worldPos">The tile's world position</param>
    /// <param name="parent"></param>
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);

        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void OnMouseOver()
    {
        //Debug.Log(GridPosition.X + " " + GridPosition.Y);
        if (transform.childCount > 0)
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnMouseExit()
    {
        if (transform.childCount > 0)
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnMouseDown()
    {
        PlaceTower();
    }

    private void PlaceTower()
    {
        Collider2D box = transform.GetComponent<Collider2D>();
        Instantiate(GameManager.Instance.TowerPrefab, new Vector3(box.bounds.center.x, box.bounds.center.y, box.bounds.center.z - 1f), Quaternion.identity);
    }
}
