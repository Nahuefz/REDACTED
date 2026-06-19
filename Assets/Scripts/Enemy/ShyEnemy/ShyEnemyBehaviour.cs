using Enemy.Core;
using UnityEngine;

namespace Enemy.ShyEnemy
{
    [RequireComponent(typeof(EnemyMotor))]
    public class ShyEnemyBehaviour : MonoBehaviour
    {
        [Header("<color=white>Waypoints del Patrullaje</color>")]
        public Transform[] patrolWaypoints;

        public Transform fleeWaypoint;

        [Space(2)]
        [Header("<color=white>Parametros de deteccion</color>")]
        public int patrolSpeed, fleeSpeed;

        [SerializeField] private float timeToGetScared;
        [Space(2)] public float waypointWaitTime = 3f;

        private IEnemyState _currentState;
        private float _contactTimer;

        public EnemyMotor Motor { get; private set; }
        public PatrolState PatrolState { get; private set; }
        public FleeState FleeState { get; private set; }

        public float DetectionProgress => timeToGetScared > 0 ? _contactTimer / timeToGetScared : 0;
        public bool IsDetectingPlayer => _contactTimer > 0;

        private void Awake()
        {
            Motor = GetComponent<EnemyMotor>();
            PatrolState = new PatrolState(this);
            FleeState = new FleeState(this);
        }

        private void Start() => TransitionToState(PatrolState);

        private void Update() => _currentState?.Update();

        public void TransitionToState(IEnemyState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        private void OnTriggerStay(Collider other)
        {
            if (_currentState != PatrolState) return;

            if (!other.CompareTag("Player")) return;

            _contactTimer += Time.deltaTime;
            EnemyEvents.RaiseShyVisibilityChanged(DetectionProgress);

            if (_contactTimer < timeToGetScared) return;

            _contactTimer = 0f;
            EnemyEvents.RaiseShyVisibilityChanged(0f);
            TransitionToState(FleeState);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            _contactTimer = 0f;
            EnemyEvents.RaiseShyVisibilityChanged(0f);
        }
    }
}
