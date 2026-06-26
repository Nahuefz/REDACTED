using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Esta es la "puerta de acceso" global
    public static CameraShake Instance { get; private set; }

    private Vector3 _posicionOriginal;

    void Awake()
    {
        // Configuramos el Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _posicionOriginal = transform.localPosition;
    }

    // El método público que llamará todo el mundo
    public void Shake(float duracion, float fuerza)
    {
        // Detiene cualquier shake previo para que no se acumulen de forma extraña
        StopAllCoroutines(); 
        StartCoroutine(DoShake(duracion, fuerza));
    }

    private IEnumerator DoShake(float duracion, float fuerza)
    {
        float tiempoTranscurrido = 0.0f;

        while (tiempoTranscurrido < duracion)
        {
            float x = Random.Range(-1f, 1f) * fuerza;
            float y = Random.Range(-1f, 1f) * fuerza;

            transform.localPosition = new Vector3(x, y, _posicionOriginal.z);
            tiempoTranscurrido += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = _posicionOriginal;
    }
}