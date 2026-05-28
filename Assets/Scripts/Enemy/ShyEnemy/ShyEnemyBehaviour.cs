using UnityEngine;
using UnityEngine.AI;

namespace Enemy.ShyEnemy
{
    public class ShyEnemyBehaviour : MonoBehaviour
    {
        //parametros
        [Header("<color=white>Waypoints del Patrullaje</color>")]
        [SerializeField] private Transform[] patrolWaypoints;
        [SerializeField] private Transform fleeWaypoint;
        [Space(2)]
        [Header("<color=white>Parametros de deteccion</color>")]
        [SerializeField] private int patrolSpeed, fleeSpeed;
        [SerializeField] private bool isScared;
        [SerializeField] private float timeToGetScared, contactTimer;
        [Space(2)]
        public NavMeshAgent aiAgent;
        [SerializeField] private StateMachine _stateMachine;
        [SerializeField] private StateMachine _currentState;
        private int _currentWaypoint;

        private void Start()
        {
            aiAgent = GetComponent<NavMeshAgent>();
            aiAgent.speed = patrolSpeed;

            if (patrolWaypoints.Length > 0)
            {
                aiAgent.SetDestination(patrolWaypoints[0].position);
            }
        }

        private void Update()
        {
            //logica de estados escalables
            if (isScared) return;
            if (patrolWaypoints.Length > 0 && !aiAgent.pathPending && aiAgent.remainingDistance <= aiAgent.stoppingDistance)
            {
                GoToNextWaypoint();
            }
        }

        void GoToNextWaypoint()
        {
            _currentWaypoint = (_currentWaypoint + 1) % patrolWaypoints.Length;
            aiAgent.SetDestination(patrolWaypoints[_currentWaypoint].position);
        }
    }
}
