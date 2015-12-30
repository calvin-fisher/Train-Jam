using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Track : Way
{
    [SerializeField]
    private Material Single, Double, Triple, Quadruple, Turn;

    public void Initialize(Tile tile, Coordinate? newConnection = null)
    {
        const float zLayer = 0.1f;

        gameObject.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + (zLayer), tile.transform.position.z);

        Tile = tile;

        if (newConnection != null)
        {
            AddConnection(newConnection.Value);
        }
    }

    protected override Way GetWayForTile(Tile tile)
    {
        return tile.Track;
    }

    #region Texture Drawing
    protected override void UpdateMaterial()
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

            default:
            case 4:
                MeshRenderer.material = Quadruple;
                break;
        }
    }

    private void UpdateMaterial1()
    {
        MeshRenderer.material = Single;

        var connection = Connections.Single();
        var direction = Coordinate.GetDirectionToNeighbor(connection.Tile.Coordinate);
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
}
