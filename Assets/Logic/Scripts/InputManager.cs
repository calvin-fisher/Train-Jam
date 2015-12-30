using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEditor;

public class InputManager : MonoBehaviour
{
    public GameObject TrackGameObject;

    public static InputManager Instance { get; private set; }

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
    }

    // Update is called once per frame
    void Update ()
	{
	    UpdateMouseoverTile();

        if (Input.GetMouseButtonDown(0) && MouseDown != null)
        {
            MouseDown(this, null);
        }

        if (Input.GetMouseButton(0) && MouseHeld != null)
        {
            MouseHeld(this, null);
        }

        if (Input.GetMouseButtonUp(0) && MouseUp != null)
        {
            MouseUp(this, null);
        }
    }

    private void UpdateMouseoverTile()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);
        
        RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        if (raycastHits.Any())
        {
            foreach (var hit in raycastHits)
            {
                var tile = hit.transform.gameObject.GetComponent<Tile>();
                if (tile != null)
                {
                    if (MouseoverTile != tile)
                    {
                        if (MouseoverTile != null)
                        {
                            MouseoverTile.CancelHighlight();
                        }

                        ProcessMouseoverTileChanged(tile);
                    }
                }
            }
        }
        else
        {
            if (MouseoverTile == null)
                return;

            ProcessMouseoverTileChanged(null);
        }
    }

    private void ProcessMouseoverTileChanged(Tile newTile)
    {
        var eventArgs = new MouseoverTileChangedEventArgs(MouseoverTile, newTile);

        if (MouseoverTile != null)
            MouseoverTile.CancelHighlight();

        MouseoverTile = newTile;

        if (MouseoverTileChanged != null)
            MouseoverTileChanged(this, eventArgs);
    }

    public Tile MouseoverTile { get; private set; }

    public static event EventHandler MouseDown;
    public static event EventHandler MouseHeld;
    public static event EventHandler MouseUp;
    public static event MouseoverTileChangedEventHandler MouseoverTileChanged;
}

public delegate void MouseoverTileChangedEventHandler(object sender, MouseoverTileChangedEventArgs e);

public class MouseoverTileChangedEventArgs : EventArgs
{
    public MouseoverTileChangedEventArgs(Tile oldTile, Tile newTile)
    {
        OldTile = oldTile;
        NewTile = newTile;
    }

    public readonly Tile OldTile;
    public readonly Tile NewTile;
}