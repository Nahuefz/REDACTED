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
        public Transform angryEnemyTransform;
        
        [Space(2)]
        [Header("<color=white>Parametros de deteccion</color>")]
        public int patrolSpeed, fleeSpeed;

        [SerializeField] private float timeToGetScared;
        [SerializeField] private float lookAtPlayerSpeed = 360f;
        [Space(2)] public float waypointWaitTime = 3f;

        private IEnemyState _currentState;
        private float _contactTimer;

        public EnemyMotor Motor { get; private set; }
        public PatrolState PatrolState { get; private set; }
        private FrozenState FrozenState { get; set; }
        private FleeState FleeState { get; set; }
        public Transform DetectedPlayer { get; private set; }

        private float DetectionProgress
        {
            get
            {
                if (timeToGetScared > 0)
                {
                    return _contactTimer / timeToGetScared;
                }

                return 0f;
            }
        }

        private void Awake()
        {
            Motor = GetComponent<EnemyMotor>();
            PatrolState = new PatrolState(this);
            FrozenState = new FrozenState(this);
            FleeState = new FleeState(this);
        }

        private void Start()
        {
            TransitionToState(PatrolState);
        }

        private void Update()
        {
            if (_currentState != null)
            {
                _currentState.Update();
            }
        }

        public void TransitionToState(IEnemyState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = newState;

            if (_currentState != null)
            {
                _currentState.Enter();
            }
        }

        public void IncreaseDetection(float deltaTime)
        {
            _contactTimer += deltaTime;
            EnemyEvents.RaiseShyVisibilityChanged(DetectionProgress);
        }

        public bool HasFinishedDetection
        {
            get { return _contactTimer >= timeToGetScared; }
        }

        public void LookAtDetectedPlayer()
        {
            if (DetectedPlayer == null) return;

            Vector3 direction = DetectedPlayer.position - transform.position;
            direction.y = 0f;

            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                lookAtPlayerSpeed * Time.deltaTime
            );
        }

        public void GetScared()
        {
            DetectedPlayer = null;
            _contactTimer = 0f;
            EnemyEvents.RaiseShyVisibilityChanged(0f);
            TransitionToState(FleeState);
        }

        public void StopDetectingPlayer()
        {
            DetectedPlayer = null;
            _contactTimer = 0f;
            EnemyEvents.RaiseShyVisibilityChanged(0f);
            TransitionToState(PatrolState);
        }

        private void OnTriggerEnter(Collider other)
        {
            TryStartDetectingPlayer(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TryStartDetectingPlayer(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_currentState != FrozenState) return;

            if (!other.CompareTag("Player")) return;

            if (other.transform == DetectedPlayer)
            {
                StopDetectingPlayer();
            }
        }

        private void TryStartDetectingPlayer(Collider other)
        {
            if (_currentState != PatrolState) return;

            if (!other.CompareTag("Player")) return;

            DetectedPlayer = other.transform;
            TransitionToState(FrozenState);
        }
    }
}
