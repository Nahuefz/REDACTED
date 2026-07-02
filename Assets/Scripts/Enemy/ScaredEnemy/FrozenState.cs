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
            bool isPlayerDetected = _enemy.DetectedPlayer != null && _enemy.CanSeeDetectedPlayer();

            if (isPlayerDetected)
            {
                _enemy.LookAtDetectedPlayer();
                _enemy.IncreaseDetection(Time.deltaTime);

                if (_enemy.HasFinishedDetection)
                {
                    _enemy.GetScared();
                }
            }
            else
            {
                _enemy.DecreaseDetection(Time.deltaTime * _enemy.ScaredDecaySpeed);

                if (_enemy.ContactTimer <= 0f)
                {
                    _enemy.StopDetectingPlayer();
                }
            }
        }

        public override void Exit()
        {
            _enemy.Motor.Resume();
        }
    }
}
