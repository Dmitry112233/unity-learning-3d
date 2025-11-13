using Bullet.Scripts;
using Common.Scripts;
using UnityEngine;

namespace Player.Scripts
{
   public class FireComponent : MonoBehaviour
   {
      [SerializeField] private PoolObject bulletPool;
      [SerializeField] private Transform spawnPoint;
      [SerializeField] private float fireRate = 0.25f;
      
      private float _nextFire;
      
      private void Update()
      {
         if (Input.GetMouseButtonDown(0) &&  Time.time > _nextFire)
         {
            _nextFire =  Time.time + fireRate;
            var bullet = bulletPool.GetObject(spawnPoint.position) as BulletController;
         }
      }
   }
}
