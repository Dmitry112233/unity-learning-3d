using Bullet.Scripts;
using UnityEngine;

namespace Asteroids.Scripts
{
    public class AsteroidManager : MonoBehaviour
    {
        [SerializeField] private float spawnRate = 3f;
        [SerializeField] private AsteroidSpawner _spawner;
      
        private float _nextFire;
        
        private void Start()
        {
            _nextFire = Time.time + spawnRate;
        }
   
        private void Update()
        {
            if (Time.time > _nextFire)
            {
                _nextFire =  Time.time + spawnRate;
                
                try
                {
                    var type = GetRandomAsteroidType();
                    _spawner.Spawn(type);
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
        
        private AsteroidType GetRandomAsteroidType()
        {
            var values = System.Enum.GetValues(typeof(AsteroidType));
            return (AsteroidType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
    }
}