using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;

public class InputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    UpdateMouseoverTile();

        var mouseButtonDown = Input.GetMouseButton(0);
        if (mouseButtonDown)
        {
	        MouseDown();
	    }
	}

    private void UpdateMouseoverTile()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);

        if (_mouseoverTile != null)
            _mouseoverTile.CancelHighlight();

        if (ModeManager.Instance.IsTrackPlacementOn)
        {
            RaycastHit[] raycastHits = Physics.RaycastAll(ray);
            if (raycastHits.Any())
            {
                Debug.Log("Raycast hit ");

                foreach (var hit in raycastHits)
                {
                    var tile = hit.transform.gameObject.GetComponent<Tile>();
                    if (tile != null)
                    {
                        tile.Highlight();
                        _mouseoverTile = tile;
                    }
                }
            }
            else
            {
                _mouseoverTile = null;
            }
        }
    }

    private void MouseDown()
    {
        if (ModeManager.Instance.IsTrackPlacementOn)
        {
            if (_mouseoverTile != null)
            {
                _mouseoverTile.LayTrack();
            }
        }
    }

    private Tile _mouseoverTile = null;

    public GameObject Track;
}
