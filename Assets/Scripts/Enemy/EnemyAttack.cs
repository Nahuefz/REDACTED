using Enemy.AngryEnemy;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private AngryEnemyBehaviour _angryEnemy;

    private void Awake()
    {
        _angryEnemy = GetComponentInParent<AngryEnemyBehaviour>();
    }

    public void DealDamage()
    {
        _angryEnemy?.DealDamageToPlayer();
    }

    public void StopAttacking()
    {
        _angryEnemy?.ResumeMovement();
    }
}
