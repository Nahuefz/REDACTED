using UnityEngine;

namespace Enemy.Core
{
    public class EnemyHealth : MonoBehaviour, IEnemyDamageable
    {
        [SerializeField, Min(1)] private int maxLife = 1;

        private int _currentLife;

        private void Awake() => _currentLife = maxLife;

        public void TakeBulletHit()
        {
            if (_currentLife > 1)
            {
                _currentLife--;
                return;
            }

            Destroy(gameObject);
        }
    }
}
