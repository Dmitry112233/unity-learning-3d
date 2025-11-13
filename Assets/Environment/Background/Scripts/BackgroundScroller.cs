using Camera.Scripts;
using UnityEngine;

namespace Environment.Background.Scripts
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] float _speed = -5;
        [SerializeField] float _length = 10;
        
        private Vector3 _startPosition;

        private void Start()
        {
            InitializeBounds();
            _startPosition = transform.position;
        }

        private void InitializeBounds()
        {
            _length = CameraBounds.Instance.Height;
        }

        void Update()
        {
            float newPosition = Mathf.Repeat(Time.time * _speed, _length);
            transform.position = _startPosition + Vector3.forward * newPosition;
        }
    }
}
