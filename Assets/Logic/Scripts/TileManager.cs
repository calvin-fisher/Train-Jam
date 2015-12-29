using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour
{
    private const int Width = 100;
    private const int Length = 100;

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
        var minX = 0 - (Width/2);
        var minY = 0 - (Width/2);
        var maxX = minX + Width;
        var maxY = minY + Length;

        Debug.Log(string.Format("Drawing {0} x {1} grid. X({2} to {3}) Y({4} to {5})",
            Width, Length, minX, maxX, minY, maxY));


        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                var newTile = GameObject.Instantiate(Tile);
                newTile.transform.position = new Vector3(x, 0, y);
            }
        }
    }


    public GameObject Tile;
}
