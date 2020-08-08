using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelManager : Singleton<LevelManager>
{
    /// <summary>
    /// The prefabs that will be added to the game that will compose the map
    /// </summary>
    [SerializeField]
    private GameObject[] tilePrefabs;

    /// <summary>
    /// The parent of the tiles that will be added
    /// </summary>
    [SerializeField]
    private Transform map;

    /// <summary>
    /// The parent of the nodes that will compose the path
    /// </summary>
    [SerializeField]
    private Transform nodeParent;

    /// <summary>
    /// The node that will compose the path
    /// </summary>
    [SerializeField]
    private Node node;

    private List<Vector3> nodePositions;

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
        CreateLevel();// places the tiles that compose the background & the node the enemies will follow
        BuildPath();
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

        Vector3 tilePos = new Vector3(worldStart.x + (TileSizeX * x), worldStart.y - (TileSizeX * y), 10);

        // set tile position
        newTile.Setup(new Point(x, y), tilePos, map);
    }

    /// <summary>
    /// Builds the path the enemies will follow during the game.
    /// </summary>
    private void BuildPath()
    {
        // get the road tiles where we will put the nodes on
        List<GameObject> roadTiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Road"));
        // path goes left to right

        /*
         * Sort the tiles by x and y.
         * 
         * Sort by ascending x positions.
         * The elements with matching x position will be sorted by ascending y.
         * 
         */
        roadTiles.Sort((a, b) =>
        {
            int ret = a.transform.position.x.CompareTo(b.transform.position.x);
            return ret != 0 ? ret : a.transform.position.y.CompareTo(b.transform.position.y);
        }
        );

        // the first node will be placed on the most left road tile
        Vector3 firstNodePos = new Vector3(roadTiles[0].transform.position.x + (TileSizeX / 2), roadTiles[0].transform.position.y - (TileSizeY / 2), roadTiles[0].transform.position.z - 2f);

        PutNode(firstNodePos);

        // start at 1 and end before the last road tile
        for (int i = 1; i < roadTiles.Count - 1; i++)
        {
            GameObject prevTile = roadTiles[i - 1];
            GameObject thisTile = roadTiles[i];
            GameObject nextTile = roadTiles[i + 1];

            /*
             * if this tile x position equals to the next tile position but the y differs
             * it means that the road goes up or down with the y(it is a column).
             * We need then to put the nodes on the corners of the road.
             * 
             */
            if (thisTile.transform.position.x == nextTile.transform.position.x &&
                thisTile.transform.position.y != nextTile.transform.position.y)
            {
                // get the elements with same x but different y
                List<GameObject> sameAxis = roadTiles.FindAll((a) => a.transform.position.x == thisTile.transform.position.x);
                /*
                 * if the tile previous to the target one is below the last one in the sorted collection
                 * then it means that the column 'goes up' therefore we will place the nodes in order,
                 * on the first tile of the sorted collection and then on the last one.
                 *
                 * The opposite will happen if the column 'goes down'.
                 * First put the node on the last tile of the sorted collection and the on the first one.
                 * 
                 * THIS IS NEEDED BECAUSE THE ORDER OF THE PLACED NODES MATTERS.
                 * THE ENEMIES WILL FOLLOW THE NODES IN THE ORDER THEY ARE PLACED ON THE SCENE.
                 */
                if (prevTile.transform.position.y < sameAxis[sameAxis.Count - 1].transform.position.y)
                {
                    // put node on first tile
                    PutNode(new Vector3(sameAxis[0].transform.position.x + (TileSizeX / 2), sameAxis[0].transform.position.y - (TileSizeY / 2), sameAxis[0].transform.position.z - 2f));
                    // put node on last tile
                    PutNode(new Vector3(sameAxis[sameAxis.Count - 1].transform.position.x + (TileSizeX / 2), sameAxis[sameAxis.Count - 1].transform.position.y - (TileSizeY / 2), sameAxis[sameAxis.Count - 1].transform.position.z - 2f));
                }
                else
                {
                    // put node on last tile
                    PutNode(new Vector3(sameAxis[sameAxis.Count - 1].transform.position.x + (TileSizeX / 2), sameAxis[sameAxis.Count - 1].transform.position.y - (TileSizeY / 2), sameAxis[sameAxis.Count - 1].transform.position.z -2f));
                    // put node on first tile
                    PutNode(new Vector3(sameAxis[0].transform.position.x + (TileSizeX / 2), sameAxis[0].transform.position.y - (TileSizeY / 2), sameAxis[0].transform.position.z - 2f));
                }
            }
        }

        // put last node
        PutNode(new Vector3(roadTiles[roadTiles.Count - 1].transform.position.x + (TileSizeX / 2), roadTiles[roadTiles.Count - 1].transform.position.y - (TileSizeY / 2), roadTiles[roadTiles.Count - 1].transform.position.z - 2f));
        SetSpawnPoint(new Vector3(nodePositions[0].x - (TileSizeX * 3), nodePositions[0].y, nodePositions[0].z));// set spawn poin off screen
    }

    /// <summary>
    /// Sets the spawn point for the enemies
    /// </summary>
    /// <param name="spawnPos">The position on which the enemies should spawn</param>
    private void SetSpawnPoint(Vector3 spawnPos)
    {
        EnemySpawner.Instance.spawnPosition = spawnPos;
        EnemySpawner.Instance.StartEnemySpawn();// after spawn point set, init enemy spawning
    }

    /// <summary>
    /// Puts a node onto the road tiles
    /// The path is on the tiles with 'Road' tag.
    /// </summary>
    /// <param name="position">The position where the node will be placed at</param>
    private void PutNode(Vector3 position)
    {
        if (nodePositions == null) nodePositions = new List<Vector3>();
        //Debug.Log(position);
        if (!nodePositions.Contains(position))
        {
            Instantiate(node, position, Quaternion.identity, nodeParent);
            nodePositions.Add(position);
        }
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
