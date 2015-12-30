using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class WayBuilder : MonoBehaviour
{
    private bool _buildingWay = false;
    private Coordinate _wayBuildingStart;
    private Pathfinding.Node _wayBuildingPathStartNode;
    private Coordinate[] _wayBuildingPath;

    private Tile MouseoverTile
    {
        get { return InputManager.Instance.MouseoverTile; }
    }

    // Use this for initialization
    void Start()
    {
        InputManager.MouseDown += OnMouseDown;
        InputManager.MouseHeld += OnMouseHeld;
        InputManager.MouseUp += OnMouseUp;
        InputManager.MouseoverTileChanged += OnMouseoverTileChanged;
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region Mouse Event Handlers
    public void OnMouseDown(object sender, EventArgs e)
    {
        switch (MenuManager.Instance.MenuMode)
        {
            case MenuMode.Track:
            case MenuMode.Road:
                OnMouseDown();
                break;
        }
    }

    private void OnMouseHeld(object sender, EventArgs e)
    {
        switch (MenuManager.Instance.MenuMode)
        {
            case MenuMode.Bulldoze:
                BulldozeMouseHeld();
                break;
        }
    }

    private void OnMouseUp(object sender, EventArgs e)
    {
        OnMouseUp();
    }

    private void OnMouseoverTileChanged(object sender, MouseoverTileChangedEventArgs e)
    {
        MouseoverUpdate(e.NewTile);
        BulldozeMouseoverUpdate(e.NewTile);
    }

    #endregion

    private void OnMouseDown()
    {
        if (!_buildingWay && MouseoverTile != null)
        {
            _buildingWay = true;
            _wayBuildingStart = MouseoverTile.Coordinate;
            MouseoverUpdate(MouseoverTile);
        }
    }

    private void MouseoverUpdate(Tile mouseoverTile)
    {
        // Always clear any previous path
        if (_wayBuildingPath != null)
        {
            foreach (var coordinate in _wayBuildingPath)
            {
                TileManager.Instance.Get(coordinate).CancelHighlight();
            }
        }

        // If mouse is off-grid, or if not laying track, exit here
        if (mouseoverTile == null 
            || (MenuManager.Instance.MenuMode != MenuMode.Track && MenuManager.Instance.MenuMode != MenuMode.Road))
        {
            _wayBuildingPath = null;
            return;
        }

        // If not currently laying track, just highlight the square
        if (!_buildingWay)
        {
            if (mouseoverTile != null)
            {
                // TODO: Highlight red instead if building is not possible
                mouseoverTile.Highlight(Color.yellow);
            }
            return;
        }

        // Update candidate path
        _wayBuildingPathStartNode = Pathfinding.FindPath(_wayBuildingStart, mouseoverTile.Coordinate);
        if (_wayBuildingPathStartNode == null)
        {
            _wayBuildingPath = null;
            return;
        }

        // Highlight new path
        _wayBuildingPath = _wayBuildingPathStartNode.SelfAndSuccessors().Select(x => x.Position).ToArray();
        foreach (var coordinate in _wayBuildingPath)
        {
            TileManager.Instance.Get(coordinate).Highlight(Color.blue);
        }
    }

    public void OnMouseUp()
    {
        if (_buildingWay)
        {
            if (_wayBuildingPath != null)
            {
                for (var i = 0; i < _wayBuildingPath.Length; i++)
                {
                    var currentCoordinate = _wayBuildingPath[i];

                    Coordinate? previousCoordinate = (i > 0)
                        ? previousCoordinate = _wayBuildingPath[i - 1]
                        : null;

                    var tile = TileManager.Instance.Get(currentCoordinate);

                    tile.CancelHighlight();
                    BuildWay(tile, previousCoordinate);
                }

                _wayBuildingPath = null;
                _wayBuildingPathStartNode = null;
            }
        }
        _buildingWay = false;
    }

    private void BuildWay(Tile tile, Coordinate? newConnection)
    {
        switch (MenuManager.Instance.MenuMode)
        {
            case MenuMode.Track:
                tile.BuildTrack(newConnection);
                break;

            case MenuMode.Road:
                tile.BuildRoad(newConnection);
                break;
        }
    }

    private void BulldozeMouseoverUpdate(Tile mouseoverTile)
    {
        if (MenuManager.Instance.MenuMode != MenuMode.Bulldoze)
            return;

        if (mouseoverTile != null)
        {
            // TODO: Highlight red instead if bulldozing is not possible
            mouseoverTile.Highlight(Color.yellow);
        }
    }

    private void BulldozeMouseHeld()
    {
        if (MouseoverTile != null)
        {
            MouseoverTile.Bulldoze();
        }
    }
}