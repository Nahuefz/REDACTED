using UnityEngine;

namespace Enemy
{
    public class ProximityBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemyBehaviour enemy;

        private void Start()
        {
            if (enemy == null) enemy = GetComponentInParent<EnemyBehaviour>();
        }
        
        //private void Update()
        
        #region OnTriggerMethods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player collided");
                enemy.currentTarget = other.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player collided exit");
                enemy.currentTarget = null;
            }
        }
        #endregion
    }
}