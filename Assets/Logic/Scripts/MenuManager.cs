using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private Toggle _trackToggle;
    private Toggle _roadToggle;
    private Toggle _bulldozeToggle;

    // Use this for initialization
    void Start ()
    {
	    if (Instance == null)
	    {
	        Instance = this;
	    }
	    else
	    {
	        GameObject.Destroy(this);
	    }

	    WireUpToggles();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.T))
	    {
	        _trackToggle.isOn = !_trackToggle.isOn;
	    }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            _bulldozeToggle.isOn = !_bulldozeToggle.isOn;
        }
	}

    private void WireUpToggles()
    {
        _trackToggle = GameObject.Find("TrackToggle").GetComponent<Toggle>();
        _roadToggle = GameObject.Find("RoadToggle").GetComponent<Toggle>();
        _bulldozeToggle = GameObject.Find("BulldozeToggle").GetComponent<Toggle>();

        _trackToggle.onValueChanged.AddListener(TrackToggled);
        _roadToggle.onValueChanged.AddListener(RoadToggled);
        _bulldozeToggle.onValueChanged.AddListener(BulldozeToggled);
    }

    public MenuMode MenuMode { get; private set; }

    private void TrackToggled(bool newValue)
    {
        if (newValue)
        {
            MenuMode = MenuMode.Track;
        }
        else
        {
            MenuMode = MenuMode.None;
        }
    }

    private void RoadToggled(bool newValue)
    {
        if (newValue)
        {
            MenuMode = MenuMode.Road;
        }
        else
        {
            MenuMode = MenuMode.None;
        }
    }

    private void BulldozeToggled(bool newValue)
    {
        if (newValue)
        {
            MenuMode = MenuMode.Bulldoze;
        }
        else
        {
            MenuMode = MenuMode.None;
        }
    }
}

public enum MenuMode
{
    None = 0,
    Track,
    Road,
    Train,
    Bulldoze,
}