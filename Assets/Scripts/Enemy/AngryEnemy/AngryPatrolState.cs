using Enemy.Core;
using UnityEngine;

namespace Enemy.AngryEnemy
{
    public class AngryPatrolState : EnemyStateBase
    {
        private readonly AngryEnemyBehaviour _enemy;

        public AngryPatrolState(AngryEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            _enemy.ResetAttackState();
            _enemy.Motor.SetSpeed(_enemy.PatrolSpeed);
            _enemy.Motor.SetAngularSpeed(120f);
            _enemy.Motor.MoveTo(_enemy.Motor.GetRandomNavMeshPoint(5f, 15f));
        }

        public override void FixedUpdate()
        {
            _enemy.Motor.UpdateLocomotionAnimation(
                _enemy.Motor.Agent.velocity.sqrMagnitude > 0.1f ? -0.5f : 0f,
                5f
            );

            if (_enemy.Motor.HasReachedDestination(_enemy.Motor.Agent.stoppingDistance + 0.1f))
                _enemy.Motor.MoveTo(_enemy.Motor.GetRandomNavMeshPoint(5f, 15f));
        }
    }
}
