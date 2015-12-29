using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    private Camera _camera;

    // Use this for initialization
    void Start()
    {
        _camera = this.GetComponent<Camera>();
    }

    private const float MoveSpeed = 2f;
    private const float ScrollSpeed = 1f;
    private Vector3 _delta;

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        Move();
        Zoom();
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

    private void Move()
    {
        transform.position += _delta * Time.deltaTime;
        _delta = Vector3.zero;
    }

    private void Zoom()
    {
        var scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0f)
        {
            _camera.orthographicSize += (-scrollWheel * ScrollSpeed);
        }
    }
}
