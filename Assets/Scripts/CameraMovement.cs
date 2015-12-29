using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    private const float MoveSpeed = 2f;
    private Vector3 _delta;

    // Update is called once per frame
    void Update()
    {
        PlayerInput();

        Move();
    }

    public void PlayerInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _delta += transform.up * MoveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _delta -= transform.right * MoveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _delta += transform.right * MoveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _delta -= transform.up * MoveSpeed;
        }
    }

    public void Move()
    {
        transform.position += _delta * Time.deltaTime;
        _delta = Vector3.zero;
    }
}
