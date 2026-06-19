using Enemy.Core;

namespace Enemy.ShyEnemy
{
    public class PatrolState : EnemyStateBase
    {
        private readonly ScaredEnemyBehaviour _enemy;
        private int _currentWaypointIndex;
        private bool _isWaiting;
        private float _waitTimer;

        public PatrolState(ScaredEnemyBehaviour enemy)
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
            if (_isWaiting)
            {
                _waitTimer += UnityEngine.Time.deltaTime;

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
        }

        public override void Exit()
        {
            _isWaiting = false;
        }

        private void SetDestinationToWaypoint()
        {
            if (_enemy.patrolWaypoints.Length == 0) return;

            _enemy.Motor.MoveTo(_enemy.patrolWaypoints[_currentWaypointIndex].position);
        }
    }
}
