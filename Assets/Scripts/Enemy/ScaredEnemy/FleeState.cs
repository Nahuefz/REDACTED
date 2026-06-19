using Enemy.Core;
using UnityEngine;

namespace Enemy.ShyEnemy
{
    public class FleeState : EnemyStateBase
    {
        private readonly ScaredEnemyBehaviour _enemy;

        public FleeState(ScaredEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            _enemy.Motor.SetSpeed(_enemy.fleeSpeed);

            if (_enemy.fleeWaypoint != null)
                _enemy.Motor.MoveTo(_enemy.angryEnemyTransform.position);
        }

        public override void Update()
        {
            if (!_enemy.Motor.HasReachedDestination()) return;

            EnemyEvents.RaiseIntruderDetected();
            _enemy.TransitionToState(_enemy.PatrolState);
        }
    }
}
