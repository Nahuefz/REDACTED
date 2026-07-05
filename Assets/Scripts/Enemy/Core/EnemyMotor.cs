using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMotor : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Animator _animator;

        public NavMeshAgent Agent
        {
            get { return _agent; }
        }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void SetSpeed(float speed)
        {
            _agent.speed = speed;
        }

        public void SetAngularSpeed(float speed)
        {
            _agent.angularSpeed = speed;
        }

        public void MoveTo(Vector3 destination)
        {
            _agent.isStopped = false;
            _agent.SetDestination(destination);
        }

        public void Stop()
        {
            _agent.isStopped = true;
        }

        public void Resume()
        {
            _agent.isStopped = false;
        }

        public bool HasReachedDestination(float threshold = 0.5f)
        {
            return !_agent.pathPending && _agent.remainingDistance <= threshold;
        }

        public void UpdateLocomotionAnimation(float targetValue, float lerpSpeed)
        {
            if (_animator == null)
            {
                Debug.LogError($"[DQA] Error: El script EnemyMotor en {gameObject.name} NO encontró ningún Animator en sus hijos.");
                return;
            }

            float current = _animator.GetFloat("xAxis");
            float next = Mathf.Lerp(current, targetValue, Time.fixedDeltaTime * lerpSpeed);

            // Este Log te va a cantar en la consola qué valor numérico le está llegando al Animator en vivo
            Debug.Log($"[DQA] Animación actualizando. Valor enviado: {next} | Animator en objeto: {_animator.gameObject.name}");

            _animator.SetFloat("xAxis", next);
        }

        public void SetAnimationTrigger(string triggerName)
        {
            if (_animator != null)
            {
                _animator.SetTrigger(triggerName);
            }
        }

        public void FaceTarget(Transform target, float rotationSpeed)
        {
            if (target == null) return;

            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0;

            if (direction == Vector3.zero) return;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.fixedDeltaTime * rotationSpeed
            );
        }

        public Vector3 GetRandomNavMeshPoint(float minRadius, float maxRadius, int attempts = 5)
        {
            for (int i = 0; i < attempts; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle * Random.Range(minRadius, maxRadius);
                Vector3 targetPos = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

                if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            return transform.position;
        }
    }
}
