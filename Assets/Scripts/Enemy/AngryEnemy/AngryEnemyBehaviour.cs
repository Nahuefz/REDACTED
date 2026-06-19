using Enemy.Core;
using UnityEngine;

namespace Enemy.AngryEnemy
{
    [RequireComponent(typeof(EnemyMotor))]
    [RequireComponent(typeof(EnemyHealth))]
    public class AngryEnemyBehaviour : MonoBehaviour
    {
        [SerializeField, Range(1f, 10f)] private float moveSpeed = 7f;
        [SerializeField, Range(0.1f, 1f)] private float patrolSpeedMultiplier = 0.3f;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerMovement player;

        private IEnemyState _currentState;

        public EnemyMotor Motor { get; private set; }
        public AngryPatrolState PatrolState { get; private set; }
        public AngryChaseState ChaseState { get; private set; }

        public float MoveSpeed => moveSpeed;
        public float PatrolSpeed => moveSpeed * patrolSpeedMultiplier;
        public float AttackRange => attackRange;
        public Transform HuntTarget { get; private set; }

        private void Awake()
        {
            Motor = GetComponent<EnemyMotor>();
            PatrolState = new AngryPatrolState(this);
            ChaseState = new AngryChaseState(this);

            ResolvePlayerReferences();
        }

        private void OnEnable() => EnemyEvents.OnIntruderDetected += TriggerHunt;

        private void OnDisable() => EnemyEvents.OnIntruderDetected -= TriggerHunt;

        private void Start() => TransitionToState(PatrolState);

        private void Update() => _currentState?.Update();

        private void FixedUpdate() => _currentState?.FixedUpdate();

        public void TransitionToState(IEnemyState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void SetHuntTarget(Transform target)
        {
            if (target == null || !target.CompareTag("Player")) return;

            HuntTarget = target;
            TransitionToState(ChaseState);
        }

        public void ClearHuntTarget()
        {
            HuntTarget = null;
            TransitionToState(PatrolState);
        }

        public void TriggerHunt()
        {
            if (playerTransform == null)
            {
                ResolvePlayerReferences();
            }

            if (playerTransform != null)
                SetHuntTarget(playerTransform);
        }

        public void ResetAttackState() => ChaseState.ResetAttack();

        public void DealDamageToPlayer()
        {
            ResetAttackState();
            player?.Respawn();
        }

        public void ResumeMovement() => Motor.Resume();

        private void ResolvePlayerReferences()
        {
            if (playerTransform != null && player == null)
                player = playerTransform.GetComponent<PlayerMovement>();

            if (playerTransform != null) return;

            if (player != null)
            {
                playerTransform = player.transform;
                return;
            }

            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null) return;

            playerTransform = playerObject.transform;
            player = playerObject.GetComponent<PlayerMovement>();
        }
    }
}
