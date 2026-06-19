using Enemy.Core;

namespace Enemy.ShyEnemy
{
    public class SearchAngryEnemyState : EnemyStateBase
    {
        private readonly ShyEnemyBehaviour _enemy;

        public SearchAngryEnemyState(ShyEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            _enemy.Motor.SetSpeed(_enemy.searchAngryEnemySpeed);

            if (_enemy.angryEnemyTransform != null)
            {
                _enemy.Motor.MoveTo(_enemy.angryEnemyTransform.position);
            }
        }

        public override void Update()
        {
            if (_enemy.angryEnemyTransform == null) return;

            if (!_enemy.Motor.HasReachedDestination()) return;

            EnemyEvents.RaiseIntruderDetected();
            _enemy.TransitionToState(_enemy.PatrolState);
        }
    }
}
