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

        //animator logic
        float animLerpSpeed = 3f;
        if (_navMeshAgent.velocity.magnitude > 0.1f)
        {
            _animator.SetFloat("xAxis", Mathf.Lerp(_animator.GetFloat("xAxis"), -1f, Time.fixedDeltaTime * animLerpSpeed));
        }
        else
        {
            _animator.SetFloat("xAxis", Mathf.Lerp(_animator.GetFloat("xAxis"), 0f, Time.fixedDeltaTime * animLerpSpeed));
        }
        
        _navMeshAgent.destination = currentTarget.position;
    }

    void PatrolState()
    {
        _navMeshAgent.speed = enemySpeed * 0.3f;
        float animLerpSpeed = 5f; 
        
        if (_navMeshAgent.velocity.sqrMagnitude > 0.1f) 
        {
            float currentX = _animator.GetFloat("xAxis");
            _animator.SetFloat("xAxis", Mathf.Lerp(currentX, -0.5f, Time.fixedDeltaTime * animLerpSpeed));
        }
        else
        {
            float currentX = _animator.GetFloat("xAxis");
            _animator.SetFloat("xAxis", Mathf.Lerp(currentX, 0f, Time.fixedDeltaTime * animLerpSpeed));
        } 

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
}
