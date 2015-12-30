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

    public void BuildTrack(Coordinate? prev, Coordinate? next)
    {
        const float zLayer = 1f;

        var newTrack = GameObject.Instantiate(TrackGameObject);
        Track = newTrack.GetComponent<Track>();
        var rotation = new Vector3();

        if (Track == null)
        {
            //TODO: Determine track type and rotation
            if (prev == null && next == null) //Single track piece
            {
                rotation = this.gameObject.transform.rotation.eulerAngles;
            }
            //Double track piece
            //Triple track piece
            //Quad track piece

            newTrack.transform.eulerAngles = rotation;
            newTrack.transform.position = new Vector3(transform.position.x, transform.position.y + (zLayer * float.Epsilon), transform.position.z);
        }
        else
        {
            //Compare existing track type to new track type and determine appropriate track type & rotation
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

    public bool CanBuildTrack
    {
        get { return Structure == null; }
    }
    public bool CanBuildStructure
    {
        get { return Structure == null; }
    }
}
