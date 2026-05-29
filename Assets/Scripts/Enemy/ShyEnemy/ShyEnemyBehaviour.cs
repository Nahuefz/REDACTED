using UnityEngine;
using UnityEngine.AI;

namespace Enemy.ShyEnemy
{
    public class ShyEnemyBehaviour : MonoBehaviour
    {
        //parametros
        [Header("<color=white>Waypoints del Patrullaje</color>")]
        public Transform[] patrolWaypoints;
        public Transform fleeWaypoint;
        [Space(2)]
        [Header("<color=white>Parametros de deteccion</color>")]
        public int patrolSpeed, fleeSpeed;
        [SerializeField] private bool isScared;
        [SerializeField] private float timeToGetScared;
        [Space(2)]
        public NavMeshAgent aiAgent;
        public float waypointWaitTime = 3f;

        public Transform player;
        private int _currentWaypoint;
        
        IEnemyState  _currentState;
        public PatrolState PatrolState { get; private set; }
        public FleeState FleeState { get; private set; }

        private void Awake()
        {
            aiAgent = GetComponent<NavMeshAgent>();
            PatrolState = new PatrolState(this);
            FleeState = new FleeState(this);
        }

        private void Start()
        {
            TransitionToState(PatrolState);
        }

        public void TransitionToState(IEnemyState newState)
        {
            _currentState?.ExitState();
            _currentState = newState;
            _currentState?.EnterState();
        }

        private void Update()
        {
            _currentState?.UpdateState();   
        }
        // Variable para el control del tiempo (ponela arriba con las demás)
        private float _contactTimer = 0f;

        private void OnTriggerStay(Collider other)
        {
            // Solo reaccionamos si estamos en PatrolState (si ya está huyendo, ignoramos)
            if (_currentState != PatrolState) return;

            if (other.CompareTag("Player"))
            {
                _contactTimer += Time.deltaTime;

                if (_contactTimer >= timeToGetScared)
                {
                    _contactTimer = 0f; // Reseteamos el reloj
                    TransitionToState(FleeState); // ¡Pánico!
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Si el jugador se aleja del collider antes de tiempo, reseteamos el contador
            if (other.CompareTag("Player"))
            {
                _contactTimer = 0f;
            }
        }
        }
    }
    public interface IEnemyState
    {
        void EnterState();  // Se ejecuta al entrar al estado
        void UpdateState(); // Se ejecuta en cada frame (reemplaza al Update de Unity)
        void ExitState();   // Se ejecuta antes de cambiar a otro estado
    }
