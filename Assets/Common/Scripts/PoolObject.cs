using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts
{
    public class PoolObject : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initialPoolSize;

        private Queue<IPoolable> _pool = new ();
        private bool _initialized = false;

        private void Start()
        {
            if (_initialized)
                return;
            
            if (prefab == null)
            {
                Debug.LogError($"Cannot initialize pool {name} — prefab is null");
                return;
            }
            
            if (prefab.GetComponent<IPoolable>() == null)
            {
                Debug.LogError($"{prefab.name} does not implement IPoolable!");
                enabled = false;
                return;
            }
            
            InitializePool();
            _initialized = true;
        }
        
        public void Initialize(GameObject prefab, Transform parent, int initialSize = 10)
        {
            this.prefab = prefab;
            transform.SetParent(parent);
            initialPoolSize = initialSize;

            if (prefab == null)
            {
                Debug.LogError($"Cannot initialize pool {name} — prefab is null");
                return;
            }

            if (prefab.GetComponent<IPoolable>() == null)
            {
                Debug.LogError($"{prefab.name} does not implement IPoolable!");
                return;
            }

            InitializePool();
            _initialized = true;
        }

        private void InitializePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewObject();
            }
        }

        private void CreateNewObject()
        {
            var go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            go.SetActive(false);
            var poolable = go.GetComponent<IPoolable>();
            _pool.Enqueue(poolable);
        }

        public IPoolable GetObject(Vector3 spawnPosition)
        {
            if (!_initialized)
            {
                Debug.LogError("PoolObject not initialized. Call Initialize(prefab, parent) first.");
                return null;
            }
            
            if (_pool.Count == 0)
            {
                CreateNewObject();
            }
            
            IPoolable obj = _pool.Dequeue();
            obj.Initialize(this, spawnPosition);
            return obj;
        }

        public void ReturnObject(IPoolable obj)
        {
            obj.Reset();
            var mb = obj as MonoBehaviour;
            if (mb != null)
            {
                mb.transform.SetParent(transform, false);
            }
            _pool.Enqueue(obj);
        }
    }
}