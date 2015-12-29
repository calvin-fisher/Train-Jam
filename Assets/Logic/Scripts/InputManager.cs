using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEditor;

public class InputManager : MonoBehaviour
{
    public GameObject TrackGameObject;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    UpdateMouseoverTile();

	    if (Input.GetMouseButtonDown(0))
	    {
	        ProcessMouseDown();
	    }

	    if (Input.GetMouseButtonUp(0))
	    {
	        ProcessMouseUp();
	    }

	}

    private void UpdateMouseoverTile()
    {
        _mouseoverTileChangedThisFrame = false;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);

        RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        if (raycastHits.Any())
        {
            foreach (var hit in raycastHits)
            {
                var tile = hit.transform.gameObject.GetComponent<Tile>();
                if (tile != null)
                {
                    if (_mouseoverTile != tile)
                    {
                        if (_mouseoverTile != null)
                        {
                            _mouseoverTile.CancelHighlight();
                        }

                        MouseoverTileChanged(tile);
                    }
                }
            }
        }
        else
        {
            if (_mouseoverTile == null)
                return;

            MouseoverTileChanged(null);
        }
    }

    private void MouseoverTileChanged(Tile newTile)
    {
        _mouseoverTileChangedThisFrame = true;

        if (_mouseoverTile != null)
            _mouseoverTile.CancelHighlight();

        if (newTile == null)
        {
            _mouseoverTile = null;
        }
        else
        {
            newTile.Highlight(Color.yellow);
            _mouseoverTile = newTile;
            //Debug.Log(string.Format("Mouseover tile ({0},{1})", _mouseoverTile.Coordinate.X, _mouseoverTile.Coordinate.Y));
        }

        if (_layingTrack) UpdateTrackLaying();
    }

    private void ProcessMouseDown()
    {
        if (ModeManager.Instance.IsTrackPlacementOn)
        {
            if (!_layingTrack && _mouseoverTile != null)
            {
                _layingTrack = true;
                _trackLayingStart = _mouseoverTile.Coordinate;
            }
        }
    }

    private void UpdateTrackLaying()
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
        if (_mouseoverTile == null || !_layingTrack)
        {
            _trackLayingPath = null;
            return;
        }

        // Update path
        _trackLayingPathStartNode = Pathfinding.FindPath(_trackLayingStart, _mouseoverTile.Coordinate);
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

    private void ProcessMouseUp()
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

    private bool _mouseoverTileChangedThisFrame = false;
    private Tile _mouseoverTile = null;
    private bool _layingTrack = false;
    private Coordinate _trackLayingStart;
    private Pathfinding.Node _trackLayingPathStartNode;
    private Coordinate[] _trackLayingPath;
}
