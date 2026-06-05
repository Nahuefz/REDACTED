using UnityEngine;

namespace Enemy.AngryEnemy
{
    [RequireComponent(typeof(EnemyBehaviour))]
    public class AngryEnemyBehaviour : MonoBehaviour
    {
        private EnemyBehaviour _enemyBehaviour;
        private GameObject _player;

        private void Awake()
        {
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        public void TriggerHunt()
        {
            if (_enemyBehaviour != null && _player != null)
            {
                Debug.Log("AngryEnemy: Hunting player!");
                _enemyBehaviour.currentTarget = _player.transform;
            }
            else
            {
                Debug.LogWarning("AngryEnemy: Cannot hunt, missing EnemyBehaviour or Player tag.");
            }
        }
    }
}
