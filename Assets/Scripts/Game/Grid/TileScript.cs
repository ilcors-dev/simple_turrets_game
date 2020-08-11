using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

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
        // show only if turrets info are not show
        if (GameManager.Instance.shownTurretInfos == null && GameManager.Instance.livesLeft != 0)
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
        // not trigger on ui clicks
        if (!IsPointerOverUIObject())
        {
            // disable turret infos on click on tile
            if (GameManager.Instance.shownTurretInfos != null)
            {
                GameManager.Instance.shownTurretInfos.SetActive(false);
                GameManager.Instance.shownTurretInfos = null;
            }

            // disable turret range on click on tile
            if (GameManager.Instance.shownRange != null)
            {
                GameManager.Instance.shownRange.enabled = false;
                GameManager.Instance.shownRange = null;
            }
            // if the turret gui is not shown, place tower
            // useful because the user could click on the map to disable the turret infos
            // and no turrets would be placed
            else
                PlaceTower();
        }
    }

    /// <summary>
    /// Places a turrett on the tile if there is not another one on it & the tile is not road
    /// </summary>
    private void PlaceTower()
    {
        if (GameManager.Instance.boughtTurret == null)// no turret to place, return
            return;

        if (!occupiedTile && !gameObject.CompareTag("Road"))
        {
            Instantiate(GameManager.Instance.boughtTurret, new Vector3(center.x, center.y, center.z - 1f), Quaternion.identity);
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
        occupiedTile = true;

        // turret placed, set it to null
        GameManager.Instance.boughtTurret = null;
    }

    /// <summary>
    /// Checks if click is over gameobject or gui
    /// </summary>
    /// <returns>false if the click was on gui, true otherwise</returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
