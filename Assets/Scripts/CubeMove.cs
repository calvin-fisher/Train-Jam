using UnityEngine;
using System.Collections;

public class CubeMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    private const float MoveSpeed = 0.5f;

	// Update is called once per frame
	void Update () {
	    if (Input.GetKey(KeyCode.W))
	    {
	        transform.position += (new Vector3(0, 0, 1))*MoveSpeed;
	    }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= (new Vector3(1, 0, 0)) * MoveSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= (new Vector3(0, 0, 1)) * MoveSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += (new Vector3(1, 0, 0)) * MoveSpeed;
        }
	}

    public void DoStuff()
    {
        Debug.Log("I did stuff!");
    }
}
