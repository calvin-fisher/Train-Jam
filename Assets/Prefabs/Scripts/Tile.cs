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

    public void LayTrack()
    {
        const float layer = 1f;

        if (Track == null)
        {
            var newTrack = GameObject.Instantiate(TrackGameObject);
            newTrack.transform.position = new Vector3(transform.position.x, transform.position.y + (layer * float.Epsilon), transform.position.z);

            Track = newTrack.GetComponent<Track>();
        }
    }

    public Track Track { get; private set; }

    public GameObject TrackGameObject;
}
