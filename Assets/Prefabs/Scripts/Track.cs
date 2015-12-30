﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Track : MonoBehaviour
{
    protected List<Track> Connections = new List<Track>();

    [SerializeField]
    private Material Single, Double, Triple, Quadruple, Turn;

    public Tile Tile { get; set; }

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
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

    }

    #region Texture Drawing
    private void UpdateMaterial()
    {
        switch (Connections.Count)
        {
            case 1:
                UpdateMaterial1();
                break;

            case 2:
                UpdateMaterial2();
                break;

            case 3:
                UpdateMaterial3();
                break;

            case 4:
                MeshRenderer.material = Quadruple;
                break;

            // TODO: Default
        }
    }

    private void UpdateMaterial1()
    {
        MeshRenderer.material = Single;

        var connection = Connections.Single();
        var direction = this.Tile.Coordinate.GetDirectionToNeighbor(connection.Tile.Coordinate);
        switch (direction)
        {
            case Direction.Up:
                this.transform.localEulerAngles = new Vector3(90, 0, 0);
                break;

            case Direction.Right:
                this.transform.localEulerAngles = new Vector3(90, 90, 0);
                break;

            case Direction.Down:
                this.transform.localEulerAngles = new Vector3(90, 180, 0);
                break;

            case Direction.Left:
                this.transform.localEulerAngles = new Vector3(90, 270, 0);
                break;

            // TODO: Default
        }
    }

    private void UpdateMaterial2()
    {
        if (Connections.TrueForAll(connection => connection.Tile.Coordinate.X == this.Tile.Coordinate.X))
        {
            // All tiles are in a row on the X axis
            MeshRenderer.material = Double;
            this.transform.localEulerAngles = new Vector3(90, 0, 0);
        }
        else if (Connections.TrueForAll(connection => connection.Tile.Coordinate.Y == this.Tile.Coordinate.Y))
        {
            // All tiles are in a row on the Y axis
            MeshRenderer.material = Double;
            this.transform.localEulerAngles = new Vector3(90, 90, 0);
        }
        else
        {
            MeshRenderer.material = Turn;
            var directions = Connections.Select(x => this.Tile.Coordinate.GetDirectionToNeighbor(x.Tile.Coordinate)).ToArray();
            if (directions.Contains(Direction.Up))
            {
                if (directions.Contains(Direction.Right))
                {
                    this.transform.localEulerAngles = new Vector3(90, 90, 0);
                }
                else //if (directions.Contains(Direction.Left))
                {
                    this.transform.localEulerAngles = new Vector3(90, 0, 0);
                }
            }
            else // if (directions.Contains(Direction.Down))
            {
                if (directions.Contains(Direction.Right))
                {
                    this.transform.localEulerAngles = new Vector3(90, 180, 0);
                }
                else //if (directions.Contains(Direction.Left))
                {
                    this.transform.localEulerAngles = new Vector3(90, 270, 0);
                }
            }
        }
    }

    private void UpdateMaterial3()
    {
        MeshRenderer.material = Triple;
        var directions = Connections.Select(x => this.Tile.Coordinate.GetDirectionToNeighbor(x.Tile.Coordinate)).ToArray();
        if (!directions.Contains(Direction.Left))
        {
            this.transform.localEulerAngles = new Vector3(90, 0, 0);
        }
        else if (!directions.Contains(Direction.Up))
        {
            this.transform.localEulerAngles = new Vector3(90, 90, 0);
        }
        else if (!directions.Contains(Direction.Right))
        {
            this.transform.localEulerAngles = new Vector3(90, 180, 0);
        }
        else //if (!directions.Contains(Direction.Down))
        {
            this.transform.localEulerAngles = new Vector3(90, 270, 0);
        }
    }
    #endregion

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
            var track = tile.Track;
            if (track != null)
            {
                if (!this.Connections.Contains(track))
                {
                    this.Connections.Add(track);
                }

                if (!track.Connections.Contains(this))
                {
                    track.Connections.Add(this);
                }

                track.UpdateMaterial();
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
