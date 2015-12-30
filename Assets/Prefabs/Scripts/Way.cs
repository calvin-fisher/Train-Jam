using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Way : MonoBehaviour {

    protected List<Way> Connections = new List<Way>();

    private MeshRenderer _meshRenderer;
    protected MeshRenderer MeshRenderer
    {
        get
        {
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();
            return _meshRenderer;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Tile Tile { get; protected set; }
    public Coordinate Coordinate { get { return Tile.Coordinate; } }

    protected abstract void UpdateMaterial();

    protected abstract Way GetWayForTile(Tile tile);

    public void AddConnection(Coordinate connection)
    {
        AddConnectionInternal(connection);

        this.UpdateMaterial();
    }

    public void AddConnections(Coordinate[] connections)
    {
        foreach (var connection in connections)
            AddConnection(connection);

        this.UpdateMaterial();
    }

    private void AddConnectionInternal(Coordinate connection)
    {
        var tile = TileManager.Instance.Get(connection);
        if (tile != null)
        {
            var way = GetWayForTile(tile);
            if (way != null)
            {
                if (!this.Connections.Contains(way))
                {
                    this.Connections.Add(way);
                }

                if (!way.Connections.Contains(this))
                {
                    way.Connections.Add(this);
                }

                way.UpdateMaterial();
            }
        }
    }

    public void Delete()
    {
        foreach (var connection in Connections)
        {
            connection.Connections.Remove(this);
            connection.UpdateMaterial();
        }
    }
}
