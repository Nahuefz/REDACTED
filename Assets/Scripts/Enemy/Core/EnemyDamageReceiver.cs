using UnityEngine;

namespace Enemy.Core
{
    public class EnemyDamageReceiver : MonoBehaviour
    {
        private IEnemyDamageable _damageable;

        private void Awake()
        {
            _damageable = GetComponentInParent<IEnemyDamageable>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
                _damageable?.TakeBulletHit();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Bullet"))
                _damageable?.TakeBulletHit();
        }
    }
}
