using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class TrackLayer : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    InputManager.MouseoverTileChanged += OnMouseoverTileChanged;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }
    }

    public void MouseDown()
    {
        if (ModeManager.Instance.IsTrackPlacementOn)
        {
            if (!_layingTrack && InputManager.Instance.MouseoverTile != null)
            {
                _layingTrack = true;
                _trackLayingStart = InputManager.Instance.MouseoverTile.Coordinate;
            }
        }
    }

    public void MouseUp()
    {
        if (_layingTrack)
        {
            if (_trackLayingPath != null)
            {
                foreach (var coordinate in _trackLayingPath)
                {
                    var tile = TileManager.Instance.Get(coordinate);
                    tile.CancelHighlight();
                    tile.LayTrack();
                }

                _trackLayingPath = null;
                _trackLayingPathStartNode = null;
            }

            _layingTrack = false;
        }
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

        // If off-grid, abort
        if (e.NewTile == null || !_layingTrack)
        {
            _trackLayingPath = null;
            return;
        }

        // Update path
        _trackLayingPathStartNode = Pathfinding.FindPath(_trackLayingStart, e.NewTile.Coordinate);
        if (_trackLayingPathStartNode == null)
        {
            Debug.Log("Error finding path to target");
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
