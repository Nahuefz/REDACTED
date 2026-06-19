using Enemy.Core;
using UnityEngine;

namespace Enemy.ShyEnemy
{
    public class FrozenState : EnemyStateBase
    {
        private readonly ScaredEnemyBehaviour _enemy;

        public FrozenState(ScaredEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            _enemy.Motor.Stop();
        }

        public override void Update()
        {
            if (_enemy.DetectedPlayer == null)
            {
                _enemy.StopDetectingPlayer();
                return;
            }

            _enemy.LookAtDetectedPlayer();
            _enemy.IncreaseDetection(Time.deltaTime);

            if (!_enemy.HasFinishedDetection) return;

            _enemy.GetScared();
        }

        public override void Exit()
        {
            _enemy.Motor.Resume();
        }
    }
}
