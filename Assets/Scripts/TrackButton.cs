using UnityEngine;
using System.Collections;

public class TrackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Pressed()
    {
        Debug.Log(gameObject.name + " pressed");

        if (!Active)
        {
            ModeManager.Instance.StartTrackPlacement();
        }
        else
        {
            ModeManager.Instance.EndTrackPlacement();
        }

        Active = !Active;
    }

    private bool Active = false;

    public GameObject Track;
}
