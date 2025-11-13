using System.Collections;
using Common.Scripts;
using UnityEngine;

namespace Asteroids.Scripts
{
    public class AsteroidExplosionController: MonoBehaviour, IPoolable
    {
        private PoolObject _pool;
        private ParticleSystem _ps;

        private void Awake()
        {
            _ps = GetComponent<ParticleSystem>();
        }

        public void Initialize(PoolObject pool, Vector3 position)
        {
            _pool = pool;
            transform.position = position;
            gameObject.SetActive(true);
            _ps.Play();
            StartCoroutine(ReturnAfter(_ps.main.duration));
        }

        private IEnumerator ReturnAfter(float delay)
        {
            yield return new WaitForSeconds(delay);
            _pool.ReturnObject(this);
        }

        public void Reset()
        {
            _ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            gameObject.SetActive(false);
        }
    }
}