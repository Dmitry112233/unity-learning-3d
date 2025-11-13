using Camera.Scripts;
using Common.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Scripts
{
    public class AsteroidController : MonoBehaviour, IPoolable, IDamageable
    {
        [SerializeField] private float speed;
        private float _rotationSpeed;
        private PoolObject _explosionsPool;
        
        public PoolObject ExplosionsPool
        { 
            set
            {
                if(!_explosionsPool) _explosionsPool = value;
            }
        }

        private PoolObject _pool;
        private Rigidbody _rb;
        private Vector3 _rotation;
        private Vector3 _direction;
        private bool _isInitialized;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            CheckBoundary();
            _rb.velocity = _direction.normalized * speed;
            _rb.angularVelocity = _rotation * _rotationSpeed;
        }

        private void CheckBoundary()
        {
            if (_isInitialized && CameraBounds.Instance.IsOutOfBounds(transform.position))
            {
                _pool.ReturnObject(this);
            }
            else if (!_isInitialized && !CameraBounds.Instance.IsOutOfBounds(transform.position))
            {
                _isInitialized = true;
            }
        }

        public void Initialize(PoolObject pool, Vector3 position)
        {
            _pool = pool;
            transform.position = position;
            transform.rotation = Quaternion.identity;
            
            speed = Random.Range(0.5f, 7f);
            _rotationSpeed = Random.Range(0.2f, 3.2f);
            _rotation = Random.insideUnitSphere;
            _direction = Vector3.back;
            
            gameObject.SetActive(true);
            _isInitialized = false;
        }

        public void Reset()
        {
            _isInitialized = false;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.rotation = Quaternion.identity;
            _rb.Sleep();
            gameObject.SetActive(false);
        }

        public void TakeDamage()
        {
            Explode();
            _pool.ReturnObject(this);
        }

        private void Explode()
        {
            _explosionsPool.GetObject(transform.position);
        }
    }
}