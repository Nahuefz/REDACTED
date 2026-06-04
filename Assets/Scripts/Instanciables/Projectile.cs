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

        // 2. Si chocamos con un Trigger (como la zona de deteccin del ShyEnemy)
        if (other.isTrigger)
        {
            // Solo nos desactivamos si el Trigger tiene un componente de enemigo
            // Esto permite atravesar zonas de deteccin pero chocar con el "cuerpo" si este es trigger
            if (other.GetComponent<Enemy.ShyEnemy.ShyEnemyBody>() != null || 
                other.GetComponent<EnemyBehaviour>() != null)
            {
                Deactivate();
            }
            return; // Si es cualquier otro trigger, lo atravesamos
        }

        // 3. Si llegamos aqu, es algo slido (Pared, Suelo, Mesh no-trigger)
        Deactivate();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
