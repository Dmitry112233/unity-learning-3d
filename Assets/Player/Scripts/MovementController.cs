using System;
using Camera.Scripts;
using Environment.Background.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Scripts
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float speed = 6f; 
        [SerializeField] private float tilt = 4f;
        
        private Vector2 _boundary;

        private Rigidbody _rb;
    
        private float _moveInputHorizontal;
        private float _moveInputVertical;
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleInput();
        }

        void FixedUpdate()
        {
            ApplyMove();
            ApplyTilt();
            ApplyBoundariesLimit();
        }


        private void HandleInput()
        {
            _moveInputHorizontal = Input.GetAxis("Horizontal");
            _moveInputVertical = Input.GetAxis("Vertical");
        }

        private void ApplyMove()
        {
            Vector3 movement = new Vector3(_moveInputHorizontal, 0.0f, _moveInputVertical);
            _rb.velocity = movement * speed;
        }
    
        private void ApplyTilt()
        {
            _rb.rotation = Quaternion.Euler(0.0f, 0.0f, _rb.velocity.x * -tilt);
        }
    
        private void ApplyBoundariesLimit()
        {
            var clampedPosition = CameraBounds.Instance.ClampToBounds(_rb.position, 1f);
            _rb.position = clampedPosition;
        }
    }
}