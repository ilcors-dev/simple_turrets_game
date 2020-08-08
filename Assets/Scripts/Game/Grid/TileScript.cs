using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    /// <summary>
    /// The position on the grid as array index
    /// </summary>
    public Point GridPosition { get; private set; }

    /// <summary>
    /// The tile width and height
    /// </summary>
    private float TileXSize, TileYSize;

    /// <summary>
    /// The center of the tile
    /// </summary>
    public Vector3 center;

    /// <summary>
    /// Signals if the tile is occupied by a turrett or not
    /// </summary>
    private bool occupiedTile = false;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer rd = GetComponent<SpriteRenderer>();
        TileXSize = rd.size.x;
        TileYSize = rd.size.y;
        center = transform.GetComponent<Collider2D>().bounds.center;
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
        if (transform.childCount > 0 && !occupiedTile)
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnMouseExit()
    {
        if (transform.childCount > 0 && !occupiedTile)
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnMouseDown()
    {
        PlaceTower();
    }

    /// <summary>
    /// Places a turrett on the tile if there is not another one on it & the tile is not road
    /// </summary>
    private void PlaceTower()
    {

        if (!occupiedTile && !gameObject.CompareTag("Road"))
        {
            Instantiate(GameManager.Instance.TowerPrefab, new Vector3(center.x, center.y, center.z - 1f), Quaternion.identity);
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
        occupiedTile = true;
    }
}
