using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private Transform map;

    /// <summary>
    /// These variables hold the up, down, left, right position of camera borders
    /// </summary>
    private float XLeftBoundPos, XRightBoundPos, YHighBoundPos, YLowBoundPos;

    /// <summary>
    /// Dictionary containing the tiles
    /// </summary>
    public Dictionary<Point, TileScript> Tiles { get; set; }

    // how big is the tile? x value
    public float TileSizeX
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    // how big is the tile? Y value
    public float TileSizeY
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y; }
    }

    // Start is called before the first frame update
    void Start()
    {
        CalculateScreenBounds();
        CreateLevel();
    }

    /// <summary>
    /// Gets screen borders positions and saves them into class variables
    /// </summary>
    private void CalculateScreenBounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;

        // get camera bounds
        Bounds bounds = new Bounds(
            Camera.main.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        // check if point where the gameobject would get instantiated is out of screen

        /**
         * x bounds
         *     |    |
         *  -> |    | <-
         *     |    |
         */
        XLeftBoundPos = bounds.extents.x - bounds.size.x;// left
        XRightBoundPos = bounds.extents.x;// right

        /** 
         * y bounds ->
         * -----
         * 
         * 
         * -----
         */
        YHighBoundPos = bounds.extents.y; // up
        YLowBoundPos = bounds.extents.y - bounds.size.y; // down
    }

    /// <summary>
    /// Check if the x and y value are in the camera viewport or not
    /// </summary>
    /// <param name="x">x point world space</param>
    /// <param name="y">y point world space</param>
    /// <returns></returns>
    private bool IsOutOfScreen(float x, float y)
    {
        if (x < XLeftBoundPos || x > XRightBoundPos || y < YLowBoundPos || y > YHighBoundPos)
            return true;

        return false;
    }

    /// <summary>
    /// Places the tiles to create the level / games tiles
    /// </summary>
    private void CreateLevel()
    {
        // init dictionary
        Tiles = new Dictionary<Point, TileScript>();

        // the rapresentation of the map level using numbers read from txt file
        // it is useful and easy to use because we can then create our levels with a txt file
        string[] mapData = ReadLevelText();

        // x map size
        int mapX = mapData[0].ToCharArray().Length;
        // y map size
        int mapY = mapData.Length;

        // get the start of the world ( top left corner )
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++)
            {
                if (!IsOutOfScreen(worldStart.x + (TileSizeX * x), worldStart.y - (TileSizeX * y)))
                    PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }
    }

    /// <summary>
    /// Places a tile based on the x and y parameters and based on the top left corner of the screen.
    /// </summary>
    /// <param name="tileType">The tile type that should be place on the map</param>
    /// <param name="x">x position of the tile in the grid</param>
    /// <param name="y">y position of the tile in the grid</param>
    /// <param name="worldStart">The top left corner of the screen</param>
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();
        //newTile.transform.SetParent(tilesHolder.transform);

        // set tile position
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSizeX * x), worldStart.y - (TileSizeX * y), 10), map);
    }

    /// <summary>
    /// Reads a text file containing the level blocks to place and returns it as a string[];
    /// </summary>
    /// <returns>array of string lines</returns>
    private string[] ReadLevelText()
    {
        TextAsset data = Resources.Load("Level") as TextAsset;

        // get string row and remove any non-numeric chars ( except '-' )
        string tmpData = data.text.Replace(Environment.NewLine, string.Empty).Replace("\r", string.Empty);

        return tmpData.Split('-');// each line ends with '-' except for last one
    }
}
