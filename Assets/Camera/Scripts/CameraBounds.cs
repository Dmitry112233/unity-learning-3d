using UnityEngine;

namespace Camera.Scripts
{
    public class CameraBounds : MonoBehaviour
    {
        public static CameraBounds Instance { get; private set; }

        
        private UnityEngine.Camera _camera;
        private float _width, _height;
        
        public float Width => _width;
        public float Height => _height;

        private void Awake()
        {
            Instance = this;
            _camera = UnityEngine.Camera.main;
            UpdateBounds();
        }

        private void UpdateBounds()
        {
            _height = _camera.orthographicSize * 2f;
            _width = _height * _camera.aspect;
        }
        
        public Vector2 GetHalfExtents() => new Vector2(_width / 2f, _height / 2f);
        
        public bool IsOutOfBounds(Vector3 position, float padding = 0f)
        {
            var half = GetHalfExtents();
            return position.x > half.x + padding ||
                   position.x < -half.x - padding ||
                   position.z > half.y + padding ||
                   position.z < -half.y - padding;
        }
        
        public Vector3 ClampToBounds(Vector3 position, float padding = 0f)
        {
            var half = GetHalfExtents();
            position.x = Mathf.Clamp(position.x, -half.x + padding, half.x - padding);
            position.z = Mathf.Clamp(position.z, -half.y + padding, half.y - padding);
            return position;
        }
    }
}
