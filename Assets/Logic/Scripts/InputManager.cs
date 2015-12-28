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

    private Tile _mouseoverTile = null;
}
