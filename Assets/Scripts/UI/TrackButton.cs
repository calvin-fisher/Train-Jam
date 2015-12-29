using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class TrackButton : MonoBehaviour
{

    private Button _button;
    private Image _image;
    private ColorBlock _colorBlock;

	// Use this for initialization
	void Start ()
	{
	    _button = this.gameObject.GetComponent<Button>();
	    _image = this.gameObject.GetComponent<Image>();
        _colorBlock = _button.colors;
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
                _image.color = Color.yellow;
            }
            else
            {
                _image.color = Color.white;
            }
            _active = !_active;
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
