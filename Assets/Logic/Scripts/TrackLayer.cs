using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class TrackLayer : MonoBehaviour
{
    private bool _layingTrack = false;
    private Coordinate _trackLayingStart;
    private Pathfinding.Node _trackLayingPathStartNode;
    private Coordinate[] _trackLayingPath;

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
                TrackPlacementMouseDown();
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
        switch (MenuManager.Instance.MenuMode)
        {
            case MenuMode.Track:
                TrackPlacementMouseUp();
                break;
        }
    }

    private void OnMouseoverTileChanged(object sender, MouseoverTileChangedEventArgs e)
    {
        TrackPlacementMouseoverUpdate(e.NewTile);
        BulldozeMouseoverUpdate(e.NewTile);
    }

    #endregion

    private void TrackPlacementMouseDown()
    {
        if (!_layingTrack && MouseoverTile != null)
        {
            _layingTrack = true;
            _trackLayingStart = MouseoverTile.Coordinate;
            TrackPlacementMouseoverUpdate(MouseoverTile);
        }
    }

    public void TrackPlacementMouseUp()
    {
        if (_layingTrack)
        {
            if (_trackLayingPath != null)
            {
                for (var i = 0; i < _trackLayingPath.Length; i++)
                {
                    var currentCoordinate = _trackLayingPath[i];

                    Coordinate? previousCoordinate = (i > 0)
                        ? previousCoordinate = _trackLayingPath[i - 1]
                        : null;

                    var tile = TileManager.Instance.Get(currentCoordinate);

                    tile.CancelHighlight();
                    tile.BuildTrack(previousCoordinate);
                }

                _trackLayingPath = null;
                _trackLayingPathStartNode = null;
            }
        }
        _layingTrack = false;
    }

    private void TrackPlacementMouseoverUpdate(Tile mouseoverTile)
    {
        // Always clear any previous path
        if (_trackLayingPath != null)
        {
            foreach (var coordinate in _trackLayingPath)
            {
                TileManager.Instance.Get(coordinate).CancelHighlight();
            }
        }

        // If mouse is off-grid, or if not laying track, exit here
        if (mouseoverTile == null || MenuManager.Instance.MenuMode != MenuMode.Track)
        {
            _trackLayingPath = null;
            return;
        }

        // If not currently laying track, just highlight the square
        if (!_layingTrack)
        {
            // TODO: Highlight red instead if building is not possible
            mouseoverTile.Highlight(Color.yellow);
            return;
        }

        // Update candidate path
        _trackLayingPathStartNode = Pathfinding.FindPath(_trackLayingStart, mouseoverTile.Coordinate);
        if (_trackLayingPathStartNode == null)
        {
            _trackLayingPath = null;
            return;
        }

        // Highlight new path
        _trackLayingPath = _trackLayingPathStartNode.SelfAndSuccessors().Select(x => x.Position).ToArray();
        foreach (var coordinate in _trackLayingPath)
        {
            TileManager.Instance.Get(coordinate).Highlight(Color.blue);
        }
    }

    private void BulldozeMouseoverUpdate(Tile mouseoverTile)
    {
        if (MenuManager.Instance.MenuMode != MenuMode.Bulldoze)
            return;

        // TODO: Highlight red instead if bulldozing is not possible
        mouseoverTile.Highlight(Color.yellow);
    }

    private void BulldozeMouseHeld()
    {
        if (MouseoverTile != null)
        {
            MouseoverTile.Bulldoze();
        }
    }
}
