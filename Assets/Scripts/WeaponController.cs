using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    private float nextFireTime;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = ObjectPooler.Instance.GetPooledObject();

        if (bullet != null)
        {
            // 1. Posicionamos la bala en la punta del cañón
            bullet.transform.position = firePoint.position;

            // 2. IMPORTANTE: Usamos la rotación de la CÁMARA, no del firePoint.
            // Esto asegura que la bala vaya exactamente hacia el centro de la pantalla.
            bullet.transform.rotation = Camera.main.transform.rotation;

            // 3. DESVINCULAR: Nos aseguramos de que la bala no tenga padre.
            // Al setear el parent en 'null', la bala vive en el espacio del mundo.
            bullet.transform.SetParent(null);

            bullet.SetActive(true);
        }
    }
}