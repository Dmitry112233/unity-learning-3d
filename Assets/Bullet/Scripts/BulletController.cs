using Camera.Scripts;
using Common.Scripts;
using UnityEngine;

namespace Bullet.Scripts
{
    public class BulletController : MonoBehaviour, IPoolable
    {
        [SerializeField] private float speed;

        private PoolObject _pool;
        private Vector2 _boundary;

        void Update()
        {
            ApplyMove();
            CheckBoundary();
        }

        private void ApplyMove()
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        private void CheckBoundary()
        {
            if (CameraBounds.Instance.IsOutOfBounds(transform.position))
            {
                _pool.ReturnObject(this);
            }
        }

        public void Initialize(PoolObject pool, Vector3 spawnPosition)
        {
            _pool = pool;
            transform.position = spawnPosition;
            gameObject.SetActive(true);
        }

        public void Reset()
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name + " collided with " + other.gameObject.name);
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
                _pool.ReturnObject(this);
            }
        }
    }
}