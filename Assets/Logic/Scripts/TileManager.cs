using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour
{
    private const int Width = 100;
    private const int Length = 100;

    public GameObject TileGameObject;

    // Use this for initialization
    void Start ()
	{
	    CreateTiles();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void CreateTiles()
    {
        var minX = 0;
        var minY = 0;
        var maxX = Width;
        var maxY = Length;

        Debug.Log(string.Format("Initializing {0} x {1} grid. X({2} to {3}) Y({4} to {5})",
            Width, Length, minX, maxX - 1, minY, maxY - 1));

        _grid = new Tile[Width,Length];

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                var newTile = GameObject.Instantiate(TileGameObject);
                newTile.transform.position = new Vector3(x, 0, y);

                var tile = newTile.GetComponent<Tile>();
                _grid[x, y] = tile;
            }
        }
    }

    private Tile[,] _grid;
    public Tile Get(int x, int y)
    {
        return _grid[x, y];
    }
}
