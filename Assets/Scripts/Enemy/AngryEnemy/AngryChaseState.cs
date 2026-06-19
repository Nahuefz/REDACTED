using Enemy.Core;
using UnityEngine;

namespace Enemy.AngryEnemy
{
    public class AngryChaseState : EnemyStateBase
    {
        private readonly AngryEnemyBehaviour _enemy;
        private bool _isAttacking;

        public AngryChaseState(AngryEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            _isAttacking = false;
            _enemy.Motor.SetSpeed(_enemy.MoveSpeed);
            _enemy.Motor.SetAngularSpeed(240f);
        }

        public override void FixedUpdate()
        {
            Transform target = _enemy.HuntTarget;
            if (target == null)
            {
                _enemy.ClearHuntTarget();
                return;
            }

            _enemy.Motor.UpdateLocomotionAnimation(
                _enemy.Motor.Agent.velocity.magnitude > 0.1f ? -1f : 0f,
                3f
            );

            float distance = Vector3.Distance(_enemy.transform.position, target.position);

            if (distance < _enemy.AttackRange)
            {
                _enemy.Motor.Stop();
                _enemy.Motor.FaceTarget(target, 10f);

                if (!_isAttacking)
                {
                    _isAttacking = true;
                    _enemy.Motor.SetAnimationTrigger("IsAttacking");
                }
            }
            else
            {
                _isAttacking = false;
                _enemy.Motor.MoveTo(target.position);
            }
        }

        public override void Exit()
        {
            _isAttacking = false;
            _enemy.Motor.Resume();
        }

        public void ResetAttack()
        {
            _isAttacking = false;
        }
    }
}
