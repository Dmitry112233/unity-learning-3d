using Common.Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class HpComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject explosionPrefab;
        
        public void TakeDamage()
        {
            Explode();
            Debug.Log("Player destroyed!");
            gameObject.SetActive(false);
        }

        private void Explode()
        {
            Instantiate(explosionPrefab,  transform.position, Quaternion.identity);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
                TakeDamage();
            }
        }
    }
}