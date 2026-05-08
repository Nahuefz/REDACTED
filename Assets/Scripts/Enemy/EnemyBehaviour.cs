using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform currentTarget;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    [SerializeField, Range(1f, 10f)] private float enemySpeed = 1f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField, Range(1, 10), Min(1)] private int enemyLife;

    private bool _isAttacking = false;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        
        _navMeshAgent.speed = enemySpeed;
    }
    
    private void FixedUpdate()
    {
        // Si no hay objetivo o no es el jugador, patrullar
        if (currentTarget == null || !currentTarget.CompareTag("Player"))
        {
            PatrolState();
        }
        else
        {
            ChaseState();
        }
    }

    void ChaseState()
    {
        _navMeshAgent.speed = enemySpeed;
        _navMeshAgent.angularSpeed = 240f;

        // Logica de animacion de movimiento
        float animLerpSpeed = 3f;
        float targetAnimValue = (_navMeshAgent.velocity.magnitude > 0.1f) ? -1f : 0f;
        _animator.SetFloat("xAxis", Mathf.Lerp(_animator.GetFloat("xAxis"), targetAnimValue, Time.fixedDeltaTime * animLerpSpeed));

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if (distanceToTarget < attackRange)
        {
            _navMeshAgent.isStopped = true;

            // Rotar hacia el jugador
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            direction.y = 0; 
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * 10f);
            }

            // Atacar solo una vez hasta que termine
            if (!_isAttacking)
            {
                _isAttacking = true;
                _animator.SetTrigger("IsAttacking");
            }
        }
        else
        {
            _isAttacking = false;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.destination = currentTarget.position;
        }
    }

    void PatrolState()
    {
        _isAttacking = false;
        _navMeshAgent.isStopped = false; 
        _navMeshAgent.speed = enemySpeed * 0.3f;
        _navMeshAgent.angularSpeed = 120f;
        float animLerpSpeed = 5f; 
        
        float targetAnimValue = (_navMeshAgent.velocity.sqrMagnitude > 0.1f) ? -0.5f : 0f;
        _animator.SetFloat("xAxis", Mathf.Lerp(_animator.GetFloat("xAxis"), targetAnimValue, Time.fixedDeltaTime * animLerpSpeed));

        // Buscar nuevo destino si ha llegado al actual
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 0.1f)
        {
            _navMeshAgent.destination = GetRoamingDir();
        }
    }

    Vector3 GetRoamingDir()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * Random.Range(5f, 15f);
            Vector3 targetPos = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPos, out hit, 5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position;
    }

    public void DealDamage()
    {
        _isAttacking = false; 
        //_navMeshAgent.isStopped = false;
    }

    public void StopAttack()
    {
        _navMeshAgent.isStopped = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet") && enemyLife > 1)
        {
            enemyLife -= 1;
            Debug.Log("BALA COLISION");
        }
        else
        {
             Destroy(gameObject);
        }
    }
}
