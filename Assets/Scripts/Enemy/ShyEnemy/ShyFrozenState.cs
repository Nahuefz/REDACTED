using Enemy.Core;
using UnityEngine;

namespace Enemy.ShyEnemy
{
    public class ShyFrozenState : EnemyStateBase
    {
        private readonly ShyEnemyBehaviour _enemy;

        public ShyFrozenState(ShyEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            _enemy.Motor.Stop();
        }

        public override void Update()
        {
            if (!_enemy.IsVisibleToPlayerCamera)
            {
                _enemy.StopDetecting();
                return;
            }

            _enemy.LookAtPlayerCamera();
            _enemy.IncreaseDetection(Time.deltaTime);

            if (!_enemy.HasFinishedDetection) return;

            _enemy.SearchAngryEnemy();
        }

        public override void Exit()
        {
            _enemy.Motor.Resume();
        }
    }
}
