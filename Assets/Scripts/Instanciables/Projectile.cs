using Enemy.Core;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    
    private float _timer;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        // Configuracin para evitar que atraviese objetos rpidos
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rb.useGravity = false;
    }

    private void OnEnable()
    {
        _timer = lifeTime;
        
        // Aplicamos la velocidad inicial
        _rb.linearVelocity = transform.forward * speed;
    }

    private void Update()
    {
        // Auto-desactivacin por tiempo
        _timer -= Time.deltaTime;
        if (_timer <= 0) Deactivate();
    }

    private void FixedUpdate()
    {
        // Mantenemos la velocidad constante (por si choca con algo que lo frene pero no lo destruya)
        _rb.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Ignorar jugador y otras balas
        if (other.CompareTag("Player") || other.CompareTag("Bullet")) return;

        if (other.GetComponent<EnemyDamageReceiver>() != null || other.GetComponent<EnemyHealth>() != null)
        {
            Deactivate();
            return;
        }

        if (other.isTrigger) return;
        Deactivate();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
