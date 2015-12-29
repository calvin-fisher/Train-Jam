using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour
{
    private const int Width = 100;
    private const int Length = 100;

    public GameObject TileGameObject;

    public static TileManager Instance { get; private set; }

    // Use this for initialization
    void Start ()
	{
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            GameObject.Destroy(this);
            return;
        }

        CreateTiles();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void CreateTiles()
    {
        MinX = 0;
        MinY = 0;
        MaxX = Width - 1;
        MaxY = Length - 1;

        Debug.Log(string.Format("Initializing {0} x {1} grid. X({2} to {3}) Y({4} to {5})",
            Width, Length, MinX, MaxX, MinY, MaxY));

        _grid = new Tile[Width,Length];

        for (int x = MinX; x <= MaxX; x++)
        {
            for (int y = MinY; y <= MaxY; y++)
            {
                var newTile = GameObject.Instantiate(TileGameObject);
                newTile.transform.position = new Vector3(x, 0, y);

                var tile = newTile.GetComponent<Tile>();
                tile.Coordinate = new Coordinate(x, y);
                _grid[x, y] = tile;
            }
        }
    }

    public int MinX { get; private set; }
    public int MaxX { get; private set; }
    public int MinY { get; private set; }
    public int MaxY { get; private set; }

    public bool IsValidCoordinate(Coordinate coordinate)
    {
        return MinX <= coordinate.X && coordinate.X <= MaxX
            && MinY <= coordinate.Y && coordinate.Y <= MaxY;
    }

    private Tile[,] _grid;
    public Tile Get(int x, int y)
    {
        return _grid[x, y];
    }

    public Tile Get(Coordinate coordinate)
    {
        return _grid[coordinate.X, coordinate.Y];
    }
}
