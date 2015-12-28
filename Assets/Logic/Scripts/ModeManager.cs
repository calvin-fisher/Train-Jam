using UnityEngine;
using System.Collections;

public class ModeManager : MonoBehaviour
{
    public static ModeManager Instance;

	// Use this for initialization
	void Start () {
	    if (Instance == null)
	    {
	        Instance = this;
	    }
	    else
	    {
	        GameObject.Destroy(this);
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsTrackPlacementOn { get; private set; }

    public void StartTrackPlacement()
    {
        Debug.Log("Starting track placement");
        IsTrackPlacementOn = true;
    }

    public void EndTrackPlacement()
    {
        Debug.Log("Ending track placement");
        IsTrackPlacementOn = false;
    }
}
