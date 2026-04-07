using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifeTime = 3f;
    private float timer;

    private void OnEnable()
    {
        timer = lifeTime;
    }

    private void Update()
    {
        // Movimiento simple hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Auto-desactivación por tiempo por si no choca con nada
        timer -= Time.deltaTime;
        if (timer <= 0) Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        Deactivate();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false); // Vuelve al cargador
    }
}