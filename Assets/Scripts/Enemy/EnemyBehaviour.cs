using System;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

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
        if (currentTarget != null) ChaseState();
        else PatrolState();
    }

    void ChaseState()
    {
        int speed = 3;
        if (_navMeshAgent.velocity.magnitude != 0)
        {
            _animator.SetFloat("xAxis", Mathf.Lerp(_animator.GetFloat("xAxis"), -1, Time.deltaTime * speed));
        }
        else
        {
            _animator.SetFloat("xAxis", Mathf.Lerp(_animator.GetFloat("xAxis"), -1, Time.deltaTime * speed));
            if(_animator.GetFloat("xAxis") < 0.1f) _animator.SetFloat("xAxis", 0);
        }
        _navMeshAgent.destination = currentTarget.position;
    }

    void PatrolState()
    {
        // que patrulle por la zona
    }
}
