using UnityEngine;

namespace Enemy.ShyEnemy
{
    public class ShyEnemyBody : MonoBehaviour
    {
        private ShyEnemyBehaviour _parent;

        private void Start()
        {
            _parent = GetComponentInParent<ShyEnemyBehaviour>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Bullet")) 
            {
                _parent.OnBulletCollision();
            }
        }
    }
}
