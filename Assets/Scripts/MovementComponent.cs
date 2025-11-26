using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private Rigidbody _rb;
    private Camera _camera;
    private Vector3 _velocity;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y));
        mousePos.y = transform.position.y;
        transform.LookAt(mousePos); 
        _rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * _speed;
    }
}
