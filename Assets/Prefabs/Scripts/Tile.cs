using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public GameObject TrackGameObject;
    public GameObject StructureGameObject;

    private MeshRenderer _meshRenderer;

    // Use this for initialization
    void Start()
    {
        _meshRenderer = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

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

    public void BuildTrack(Coordinate? newConnection = null)
    {
        const float zLayer = 0.1f;

        if (Track != null)
        {
            if (newConnection != null)
            {
                Track.AddConnection(newConnection.Value);
            }
            return;
        }

        var newTrackGameObject = GameObject.Instantiate(TrackGameObject);
        newTrackGameObject.transform.position = new Vector3(transform.position.x, transform.position.y + (zLayer), transform.position.z);

        Track = newTrackGameObject.GetComponent<Track>();
        Track.Initialize(this, newConnection);
        // TODO: Refactor track builder so that connections are updated all at once at the end, instead of having to be updated repeatedly throughout
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
            Track.Delete();
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

    public Coordinate Coordinate { get; set; }
    public Track Track { get; private set; }
    public Road Road { get; private set; }
    public Structure Structure { get; private set; }

    public bool CanBuildTrack
    {
        get { return Structure == null; }
    }
    public bool CanBuildRoad
    {
        get { return Road == null; }
    }
    public bool CanBuildStructure
    {
        get { return Structure == null; }
    }
}
