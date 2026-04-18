using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    private PlayerInputs shootInput;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    private float nextFireTime;

    private void Awake()
    {
        shootInput = new PlayerInputs();
    }

    private void Start()
    {
        shootInput.Enable();
    }

    void Update()
    {
        /*if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            Debug.Log("Disparo BANG BANG");
            nextFireTime = Time.time + fireRate;
        }*/

        if (shootInput.Player.Shoot.WasPerformedThisFrame())
        {
            //Debug.Log("<color=white>boton presionado</color>");
            Shoot();
            Debug.Log("Disparo <color=red>BANG BANG</color>");
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = ObjectPooler.Instance.GetPooledObject();

        if (bullet != null)
        {
            // 1. Posicionamos la bala en la punta del ca��n
            bullet.transform.position = firePoint.position;

            // 2. IMPORTANTE: Usamos la rotaci�n de la C�MARA, no del firePoint.
            // Esto asegura que la bala vaya exactamente hacia el centro de la pantalla.
            bullet.transform.rotation = Camera.main.transform.rotation;

            // 3. DESVINCULAR: Nos aseguramos de que la bala no tenga padre.
            // Al setear el parent en 'null', la bala vive en el espacio del mundo.
            bullet.transform.SetParent(null);

            bullet.SetActive(true);
        }
    }
}