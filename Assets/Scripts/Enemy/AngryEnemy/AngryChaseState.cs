using Enemy.Core;
using UnityEngine;

namespace Enemy.AngryEnemy
{
    public class AngryChaseState : EnemyStateBase
    {
        private readonly AngryEnemyBehaviour _enemy;
        private bool _isAttacking;

        // Variables para el control de pasos del Gigante
        private float _stepTimer;
        private const float StepInterval = 0.6f; // Tiempo en segundos entre cada pisada
        private const float MaxShakeRadius = 25f; // Distancia máxima para empezar a sentir el temblor
        private const float MaxShakeForce = 0.25f; // Fuerza máxima en la cara del jugador
        private const float ShakeDuration = 0.15f; // Duración de cada sacudida

        public AngryChaseState(AngryEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            _isAttacking = false;
            _enemy.Motor.SetSpeed(_enemy.MoveSpeed);
            _enemy.Motor.SetAngularSpeed(240f);
            _stepTimer = 0f; // Inicializa el temporizador al entrar al estado
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

            // --- LÓGICA DE TEMBLOR POR PISADAS GIGANTES ---
            // Solo tiembla si el enemigo se está moviendo activamente (velocidad > 0.1f) y no está atacando
            if (_enemy.Motor.Agent.velocity.magnitude > 0.1f && distance > _enemy.AttackRange)
            {
                _stepTimer += Time.fixedDeltaTime;

                if (_stepTimer >= StepInterval)
                {
                    TriggerFootstepShake(distance);
                    _stepTimer = 0f; // Reinicia el ritmo
                }
            }
            // ----------------------------------------------

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

        private void TriggerFootstepShake(float distance)
        {
            // Si el jugador está demasiado lejos, ni nos molestamos en calcular
            if (distance > MaxShakeRadius) return;

            // Invertimos la distancia para que a menor distancia, mayor sea la fuerza (rango 0 a 1)
            float proximityPercentage = 1f - (distance / MaxShakeRadius);
            proximityPercentage = Mathf.Clamp01(proximityPercentage);

            // Multiplicamos por la fuerza tope configurada
            float finalForce = proximityPercentage * MaxShakeForce;

            // Disparamos el Singleton de la cámara que creamos antes
            if (CameraShake.Instance != null)
            {
                CameraShake.Instance.Shake(ShakeDuration, finalForce);
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
