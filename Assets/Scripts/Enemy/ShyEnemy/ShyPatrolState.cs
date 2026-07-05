using Enemy.Core;
using UnityEngine;

namespace Enemy.ShyEnemy
{
    public class ShyPatrolState : EnemyStateBase
    {
        private readonly ShyEnemyBehaviour _enemy;
        private int _currentWaypointIndex;
        private bool _isWaiting;
        private float _waitTimer;

        public ShyPatrolState(ShyEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            _isWaiting = false;
            _waitTimer = 0f;
            _enemy.Motor.SetSpeed(_enemy.patrolSpeed);
            SetDestinationToWaypoint();
        }

        public override void Update()
        {
            if (_enemy.TryFreezeFromCameraVisibility()) return;

            if (_enemy.patrolWaypoints == null || _enemy.patrolWaypoints.Length == 0) return;

            if (_isWaiting)
            {
                _waitTimer += Time.deltaTime;

                // Forzamos la animación a Idle (0) de forma fluida mientras espera en el punto
                _enemy.Motor.UpdateLocomotionAnimation(0f, 5f);

                if (_waitTimer >= _enemy.waypointWaitTime)
                {
                    _isWaiting = false;
                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _enemy.patrolWaypoints.Length;
                    SetDestinationToWaypoint();
                }

                return;
            }

            if (_enemy.Motor.HasReachedDestination())
            {
                _isWaiting = true;
                _waitTimer = 0f;
            }

            // Cambiado '_behaviour' por '_enemy' para que compile perfectamente
            float velocidadReal = _enemy.Motor.Agent.velocity.magnitude / _enemy.Motor.Agent.speed;

            // Enviamos la velocidad al Blend Tree
            _enemy.Motor.UpdateLocomotionAnimation(velocidadReal, 5f);
        }

        public override void Exit()
        {
            _isWaiting = false;
        }

        private void SetDestinationToWaypoint()
        {
            if (_enemy.patrolWaypoints == null || _enemy.patrolWaypoints.Length == 0) return;

            _enemy.Motor.MoveTo(_enemy.patrolWaypoints[_currentWaypointIndex].position);
        }
    }
}