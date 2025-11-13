using System;
using Camera.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Scripts
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private AsteroidFactory factory;
        private Vector2 _boundary;

        private void Start()
        {
            HandleBoundsUpdate();
        }

        public void Spawn(AsteroidType type)
        {
            var position = new Vector3(Random.Range(-_boundary.x, _boundary.x), 0.0f,  _boundary.y + 2f); 
            factory.Create(type, position);
        }
        
        private void HandleBoundsUpdate()
        {
            var half = CameraBounds.Instance.GetHalfExtents();
            _boundary.x = CameraBounds.Instance.Width / 2 - 1;
            _boundary.y = CameraBounds.Instance.Height / 2;
        }
    }
}