using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        _meshRenderer = this.GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private MeshRenderer _meshRenderer;

    public void Highlight()
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material.color = Color.yellow;
        }
    }

    public void CancelHighlight()
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material.color = Color.white;
        }
    }
}
