using System.Collections.Generic;
using System.Linq;
using Common.Scripts;
using UnityEngine;

namespace Asteroids.Scripts
{
    public class AsteroidFactory : MonoBehaviour
    {
        [SerializeField] private List<AsteroidConfig> configs;
        [SerializeField] private PoolObject _explosionsPools;
        Dictionary<AsteroidType, PoolObject> _asteroidPools;
        
        private void Start()
        {
            _asteroidPools = new Dictionary<AsteroidType, PoolObject>();
        }

        public AsteroidController Create(AsteroidType type, Vector3 position)
        {
            AsteroidConfig selectedConfig = configs.FirstOrDefault(config => config.type == type);
            
            if (selectedConfig == null)
            {
                Debug.LogError($"No config found for {type}");
                return null;
            }
            
            if (!_asteroidPools.TryGetValue(type, out var pool))
            {
                pool = CreatePool(selectedConfig.prefab, type);
                _asteroidPools[type] = pool;
            }
            
            var asteroid = pool.GetObject(position) as MonoBehaviour;

            asteroid!.TryGetComponent<AsteroidController>(out var asteroidController);
            asteroidController.ExplosionsPool =  _explosionsPools;
            return asteroidController;
        }
        
        private PoolObject CreatePool(GameObject prefab, AsteroidType key)
        {
            var poolGo = new GameObject($"{key}_Pool");
            poolGo.transform.SetParent(transform);
            
            var pool = poolGo.AddComponent<PoolObject>();
            pool.Initialize(prefab, poolGo.transform, initialSize: 10);
            return pool;
        }
    }
    
    public enum AsteroidType
    {
        First,
        Second,
        Third
    }
}
