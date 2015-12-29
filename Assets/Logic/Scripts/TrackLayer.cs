using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class TrackLayer : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	    InputManager.MouseoverTileChanged += OnMouseoverTileChanged;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownEvent();
        }

	    if (Input.GetMouseButton(0))
	    {
	        MouseDownHeld();
	    }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUpEvent();
        }
    }

    public void MouseDownEvent()
    {
        switch (MenuManager.Instance.MenuMode)
        {
            case MenuMode.Track:
                TrackPlacementMouseDown();
                break;
        }
    }

    private void TrackPlacementMouseDown()
    {
        if (!_layingTrack && MouseoverTile != null)
        {
            _layingTrack = true;
            _trackLayingStart = MouseoverTile.Coordinate;
        }
    }

    private void BulldozeMouseDown()
    {
        if (MouseoverTile != null)
        {
            MouseoverTile.Bulldoze();
        }
    }

    private void MouseDownHeld()
    {
        switch (MenuManager.Instance.MenuMode)
        {
            case MenuMode.Bulldoze:
                BulldozeMouseDown();
                break;
        }
    }

    private void MouseUpEvent()
    {
        switch (MenuManager.Instance.MenuMode)
        {
            case MenuMode.Track:
                TrackPlacementMouseUp();
                break;
        }
    }

    public void TrackPlacementMouseUp()
    {
        if (_layingTrack)
        {
            if (_trackLayingPath != null)
            {
                foreach (var coordinate in _trackLayingPath)
                {
                    var tile = TileManager.Instance.Get(coordinate);
                    tile.CancelHighlight();
                    tile.BuildTrack();
                }

                _trackLayingPath = null;
                _trackLayingPathStartNode = null;
            }

            _layingTrack = false;
        }
    }

    private Tile MouseoverTile
    {
        get { return InputManager.Instance.MouseoverTile; }
    }

    private void OnMouseoverTileChanged(object sender, MouseoverTileChangedEventArgs e)
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
        if (e.NewTile == null || !_layingTrack || !MenuManager.Instance.IsTrackPlacementOn)
        {
            _trackLayingPath = null;
            return;
        }

        // Update path
        _trackLayingPathStartNode = Pathfinding.FindPath(_trackLayingStart, e.NewTile.Coordinate);
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

    private bool _layingTrack = false;
    private Coordinate _trackLayingStart;
    private Pathfinding.Node _trackLayingPathStartNode;
    private Coordinate[] _trackLayingPath;

}
