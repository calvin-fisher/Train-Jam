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
	    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);

	    RaycastHit raycastHit;
	    if (Physics.Raycast(ray, out raycastHit))
	    {
	        Debug.Log("Raycast hit ");

	        if (raycastHit.transform.gameObject != null)
	        {
	            // Unhighlight previous
	            if (_mouseoverTile != null)
	            {
	                _mouseoverTile.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
	            }

	            // Highlight
	            _mouseoverTile = raycastHit.transform.gameObject;
	            _mouseoverTile.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
	        }
	    }
	    else
	    {
            _mouseoverTile.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    private GameObject _mouseoverTile = null;
}
