using Enemy.Core;

namespace Enemy.ShyEnemy
{
    public class FleeState : EnemyStateBase
    {
        private readonly ShyEnemyBehaviour _enemy;

        public FleeState(ShyEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            _enemy.Motor.SetSpeed(_enemy.fleeSpeed);

            if (_enemy.fleeWaypoint != null)
                _enemy.Motor.MoveTo(_enemy.fleeWaypoint.position);
        }

        public override void Update()
        {
            if (!_enemy.Motor.HasReachedDestination()) return;

            EnemyEvents.RaiseIntruderDetected();
            _enemy.TransitionToState(_enemy.PatrolState);
        }
    }
}
