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
	    UpdateHighlightedTile();

	    if (Input.GetMouseButtonDown(0))
	    {
	        MouseDown();
	    }
	}

    private void UpdateHighlightedTile()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);

        if (_mouseoverTile != null)
            _mouseoverTile.CancelHighlight();

        if (ModeManager.Instance.IsTrackPlacementOn)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                Debug.Log("Raycast hit ");
                var hitTile = raycastHit.transform.gameObject.GetComponent<Tile>();

                if (hitTile != null)
                {

                    hitTile.Highlight();
                    _mouseoverTile = hitTile;
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
                var newTrack = GameObject.Instantiate(Track);
                newTrack.transform.position = _mouseoverTile.transform.position;
            }
        }
    }

    private Tile _mouseoverTile = null;

    public GameObject Track;
}
