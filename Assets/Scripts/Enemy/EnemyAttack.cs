using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour;

    private void Awake()
    {
        // Corregido: asignamos la referencia correctamente
        _enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
    }

    public void DealDamage()
    {
        if (_enemyBehaviour != null)
        {
            _enemyBehaviour.DealDamage();
        }
    }
}
