using System;
using UnityEngine;
using System.Collections;

public enum TrackType
{
    Single,
    Double,
    Triple,
    Quadrople,
    Turn
}

public class Track : MonoBehaviour
{
    private TrackType _trackType;
    public TrackType TrackType
    {
        set
        {
            if (value != _trackType)
            {
                _trackType = value;
                UpdateMaterial();
            }
        }
    }

    [SerializeField]
    private Material Single, Double, Triple, Quadruple, Turn;

    private Material _currentMaterial;

    // Use this for initialization
    private void Start()
    {
        _currentMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void UpdateMaterial()
    {
        switch (_trackType)
        {
            case TrackType.Single:
                _currentMaterial = Single;
                break;
            case TrackType.Double:
                _currentMaterial = Double;
                break;
            case TrackType.Triple:
                _currentMaterial = Triple;
                break;
            case TrackType.Quadrople:
                _currentMaterial = Quadruple;
                break;
            case TrackType.Turn:
                _currentMaterial = Turn;
                break;
        }
    }
}
