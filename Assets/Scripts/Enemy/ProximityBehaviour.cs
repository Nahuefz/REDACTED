using Enemy.AngryEnemy;
using UnityEngine;

namespace Enemy
{
    public class ProximityBehaviour : MonoBehaviour
    {
        [SerializeField] private AngryEnemyBehaviour angryEnemy;

        private void Awake()
        {
            if (angryEnemy == null)
                angryEnemy = GetComponentInParent<AngryEnemyBehaviour>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                angryEnemy?.SetHuntTarget(other.transform);
        }

        // private void OnTriggerExit(Collider other)
        // {
        //     if (other.CompareTag("Player"))
        //         angryEnemy?.ClearHuntTarget();
        // }
    }
}
