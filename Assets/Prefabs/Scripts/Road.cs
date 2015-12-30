using UnityEngine;
using System.Collections;

public class Road : Way
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void UpdateMaterial()
    {
        throw new System.NotImplementedException();
    }

    protected override Way GetWayForTile(Tile tile)
    {
        return tile.Road;
    }
}
