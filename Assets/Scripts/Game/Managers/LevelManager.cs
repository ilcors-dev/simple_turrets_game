using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tilePrefabs;

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

    private GameObject tilesHolder;

    // Start is called before the first frame update
    void Start()
    {
        tilesHolder = GameObject.FindGameObjectWithTag("TilesHolder");
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Places the tiles to create the level / games tiles
    /// </summary>
    private void CreateLevel()
    {
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
                Debug.Log("x : " + x + " y : " + y);
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }
    }

    /// <summary>
    /// Places a tile based on the x and y parameters and based on the top left corner of the screen.
    /// </summary>
    /// <param name="tileType">The tile type that should be place on the map</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="worldStart">The top left corner of the screen</param>
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        newTile.transform.SetParent(tilesHolder.transform);
        newTile.transform.position = new Vector3(worldStart.x + (TileSizeX * x), worldStart.y - (TileSizeX * y), 3);
    }

    /// <summary>
    /// Reads a text file containing the level blocks to place and returns it as a string[];
    /// </summary>
    /// <returns>array of string lines</returns>
    private string[] ReadLevelText()
    {
        TextAsset data = Resources.Load("Level") as TextAsset;

        string tmpData = data.text.Replace(Environment.NewLine, string.Empty);

        return tmpData.Split('-');// each line ends with '-' except for last one
    }
}
