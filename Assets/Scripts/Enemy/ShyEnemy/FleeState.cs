using System;
using Unity.Mathematics;
using UnityEngine;

namespace Enemy.ShyEnemy
{
    public class FleeState : IEnemyState
    {
        private readonly ShyEnemyBehaviour _enemy;

        public FleeState(ShyEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public void EnterState()
        {
            // 1. Le subimos la velocidad para que corra
            _enemy.aiAgent.speed = _enemy.fleeSpeed;

            // 2. Lo mandamos directo a la guarida
            if (_enemy.fleeWaypoint != null)
            {
                _enemy.aiAgent.SetDestination(_enemy.fleeWaypoint.position);
            }

            Debug.Log("¡A correr! Huyendo a la guarida...");
        }

        public void UpdateState()
        {
            // Chequeamos si ya llegó a la guarida
            if (!_enemy.aiAgent.pathPending && _enemy.aiAgent.remainingDistance < 0.5f)
            {
                // Llegó a salvo.
                Debug.Log("Llegué a la guarida, ¡busquen al intruso!");
                
                // Trigger AngryEnemy
                if (_enemy.angryEnemy != null)
                {
                    _enemy.angryEnemy.TriggerHunt();
                }
                else
                {
                    Debug.LogWarning("ShyEnemy: No AngryEnemy assigned to trigger!");
                }

                _enemy.TransitionToState(_enemy.PatrolState);
            }
        }

        public void ExitState()
        {
            // Limpieza al salir del estado de pánico (si hace falta)
            Debug.Log("Ya me calé, vuelvo a mis asuntos.");
        }
    }
}