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
        TrackPlacementHighlightingUpdate(e.NewTile);
    }
    #endregion

    private void TrackPlacementMouseDown()
    {
        if (!_layingTrack && MouseoverTile != null)
        {
            _layingTrack = true;
            _trackLayingStart = MouseoverTile.Coordinate;
            TrackPlacementHighlightingUpdate(MouseoverTile);
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

    private void TrackPlacementHighlightingUpdate(Tile mouseoverTile)
    {
        // Clear previous path
        if (_trackLayingPath != null)
        {
            foreach (var coordinate in _trackLayingPath)
            {
                TileManager.Instance.Get(coordinate).CancelHighlight();
            }
        }

        // If off-grid or cancelled, abort
        if (mouseoverTile == null || !_layingTrack || !MenuManager.Instance.IsTrackPlacementOn)
        {
            _trackLayingPath = null;
            return;
        }

        // Update path
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

    private void BulldozeMouseHeld()
    {
        if (MouseoverTile != null)
        {
            MouseoverTile.Bulldoze();
        }
    }
}
