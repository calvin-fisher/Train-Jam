using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
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
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Length; y++)
            {
                var newTile = GameObject.Instantiate(Tile);
                newTile.transform.position = new Vector3(x, 0, y);
            }
        }
    }


    public GameObject Tile;
}
