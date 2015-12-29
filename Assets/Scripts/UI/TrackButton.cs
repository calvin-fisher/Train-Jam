using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrackButton : MonoBehaviour
{

    private Button _button;
    private ColorBlock _colorBlock;

	// Use this for initialization
	void Start ()
	{
	    _button = this.gameObject.GetComponent<Button>();
        _colorBlock = _button.colors;;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private bool _active = false;
    public bool Active
    {
        get { return _active; }
        set
        {
            if (value)
            {
                _colorBlock.normalColor = Color.yellow;
            }
            else
            {
                _colorBlock.normalColor = Color.white;
            }
        }
    }

    public void Pressed()
    {
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

    public GameObject Track;
}
