using Enemy.Core;
using UnityEngine;

namespace Enemy.ShyEnemy
{
    [RequireComponent(typeof(EnemyMotor))]
    public class ShyEnemyBehaviour : MonoBehaviour
    {
        [Header("<color=white>Waypoints del Patrullaje</color>")]
        public Transform[] patrolWaypoints;
        public Transform angryEnemyTransform;

        [Space(2)]
        [Header("<color=white>Parametros de movimiento</color>")]
        public float patrolSpeed = 2f;
        public float searchAngryEnemySpeed = 5f;

        [Space(2)]
        [Header("<color=white>Parametros de deteccion</color>")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform visibilityPoint;
        [SerializeField] private LayerMask visionBlockerMask;
        [SerializeField] private float timeToGetScared = 2f;
        [SerializeField] private float lookAtCameraSpeed = 360f;
        [Space(2)] public float waypointWaitTime = 3f;

        private IEnemyState _currentState;
        private float _visibilityTimer;

        public EnemyMotor Motor { get; private set; }
        public ShyPatrolState PatrolState { get; private set; }
        private ShyFrozenState FrozenState { get; set; }
        private SearchAngryEnemyState SearchAngryEnemyState { get; set; }

        public bool IsVisibleToPlayerCamera
        {
            get
            {
                ResolvePlayerCamera();

                if (playerCamera == null) return false;

                Vector3 targetPosition = visibilityPoint != null ? visibilityPoint.position : transform.position;
                Vector3 viewportPoint = playerCamera.WorldToViewportPoint(targetPosition);

                bool isInsideCameraView = viewportPoint.z > 0f &&
                                          viewportPoint.x >= 0f && viewportPoint.x <= 1f &&
                                          viewportPoint.y >= 0f && viewportPoint.y <= 1f;

                if (!isInsideCameraView) return false;

                return !IsViewBlocked(targetPosition);
            }
        }

        public bool HasFinishedDetection
        {
            get { return _visibilityTimer >= timeToGetScared; }
        }

        private float DetectionProgress
        {
            get
            {
                if (timeToGetScared > 0f)
                {
                    return _visibilityTimer / timeToGetScared;
                }

                return 0f;
            }
        }

        private void Awake()
        {
            Motor = GetComponent<EnemyMotor>();
            PatrolState = new ShyPatrolState(this);
            FrozenState = new ShyFrozenState(this);
            SearchAngryEnemyState = new SearchAngryEnemyState(this);
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

        public bool TryFreezeFromCameraVisibility()
        {
            if (_currentState != PatrolState) return false;

            if (IsVisibleToPlayerCamera)
            {
                TransitionToState(FrozenState);
                return true;
            }

            return false;
        }

        public void IncreaseDetection(float deltaTime)
        {
            _visibilityTimer += deltaTime;
            EnemyEvents.RaiseShyVisibilityChanged(DetectionProgress);
        }

        public void LookAtPlayerCamera()
        {
            ResolvePlayerCamera();

            if (playerCamera == null) return;

            Vector3 direction = playerCamera.transform.position - transform.position;
            direction.y = 0f;

            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                lookAtCameraSpeed * Time.deltaTime
            );
        }

        public void StopDetecting()
        {
            _visibilityTimer = 0f;
            EnemyEvents.RaiseShyVisibilityChanged(0f);
            TransitionToState(PatrolState);
        }

        public void SearchAngryEnemy()
        {
            _visibilityTimer = 0f;
            EnemyEvents.RaiseShyVisibilityChanged(0f);
            TransitionToState(SearchAngryEnemyState);
        }

        private void ResolvePlayerCamera()
        {
            if (playerCamera != null) return;

            playerCamera = Camera.main;
        }

        private bool IsViewBlocked(Vector3 targetPosition)
        {
            Vector3 cameraPosition = playerCamera.transform.position;

            return Physics.Linecast(
                cameraPosition,
                targetPosition,
                visionBlockerMask,
                QueryTriggerInteraction.Ignore
            );
        }
    }
}
