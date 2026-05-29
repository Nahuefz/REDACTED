using System.Collections;
using UnityEngine;

namespace Enemy.ShyEnemy
{
    public class PatrolState : IEnemyState
    {
        private readonly ShyEnemyBehaviour _enemy;
        private int  _currentWaypointIndex;
        private float _scaredTimer;

        private bool _isWaiting;
        private float _waitTimer;
        public PatrolState(ShyEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }
        public void EnterState()
        {
            _isWaiting = false;
            _waitTimer = 0f;
            _enemy.aiAgent.speed = _enemy.patrolSpeed;
            _scaredTimer = 0f;
            SetDestinationToWaypoint();
        }
        public void UpdateState()
        {
            // 1. Si está esperando, corre el reloj
            if (_isWaiting)
            {
                _waitTimer += Time.deltaTime;
            
                // Suponiendo que agregás "waypointWaitTime" en tu EnemyAI (MonoBehaviour)
                if (_waitTimer >= _enemy.waypointWaitTime)
                {
                    _isWaiting = false;
                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _enemy.patrolWaypoints.Length;
                    SetDestinationToWaypoint();
                }
            
                // Importante: retornamos acá para que no ejecute la lógica de abajo mientras espera
                return; 
            }

            // 2. Lógica normal de patrulla: chequear si llegó al waypoint
            if (!_enemy.aiAgent.pathPending && _enemy.aiAgent.remainingDistance < 0.5f)
            {
                // Llegó a destino: activamos la espera y reseteamos el timer
                _isWaiting = true;
                _waitTimer = 0f;
            
                // Opcional: Podés frenar el agente acá si querés que se quede clavado en el lugar
                // _enemy.agent.ResetPath(); 
            }
        }

        public void ExitState()
        {
            _isWaiting = false;
        }
        private void SetDestinationToWaypoint()
        {
            if (_enemy.patrolWaypoints.Length > 0)
            {
                _enemy.aiAgent.SetDestination(_enemy.patrolWaypoints[_currentWaypointIndex].position);
            }
        }
    }
}
