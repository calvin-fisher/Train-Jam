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

    public void Highlight(Color color)
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material.color = color;
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
        const float zLayer = 1f;

        if (Track == null)
        {
            var newTrack = GameObject.Instantiate(TrackGameObject);
            newTrack.transform.position = new Vector3(transform.position.x, transform.position.y + (zLayer * float.Epsilon), transform.position.z);

            Track = newTrack.GetComponent<Track>();
        }
    }

    public void BuildStructure()
    {
        const float zLayer = 2f;

        if (Structure == null)
        {
            var newStructure = GameObject.Instantiate(StructureGameObject);
            newStructure.transform.position = new Vector3(transform.position.x, transform.position.y + (zLayer * float.Epsilon), transform.position.z);

            Structure = newStructure.GetComponent<Structure>();
        }
    }

    public void Bulldoze()
    {
        if (Track != null)
        {
            GameObject.Destroy(Track.gameObject);
            Track = null;
        }

        if (Structure != null)
        {
            if (Structure.CanBulldoze)
            {
                GameObject.Destroy(Structure.gameObject);
                Structure = null;
            }
        }
    }

    public Track Track { get; private set; }
    public Structure Structure { get; private set; }
    public Coordinate Coordinate { get; set; }

    public GameObject TrackGameObject;
    public GameObject StructureGameObject;
}
