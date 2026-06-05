using UnityEngine;
using System.Collections;
public class RataArchivo : MonoBehaviour
{
    [Header("Configuracion de Movimiento")]
    public float velocidad = 8f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] sonidoCorriendo;

    private bool moving = false;

    void Start()
    {
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void EscapeTo(Transform nuevoPunto)
    {
        if (!moving)
        {
            StartCoroutine(RutinaEscapar(nuevoPunto));
        }
    }
    
    private IEnumerator RutinaEscapar(Transform destino)
    {
        moving = true;

        if (audioSource != null && sonidoCorriendo.Length > 0)
        {
            int indice = Random.Range(0, sonidoCorriendo.Length);
            audioSource.clip = sonidoCorriendo[indice];
            audioSource.Play();
        }

        Vector3 puntoAMirar = new Vector3(destino.position.x, destino.position.y, destino.position.z);
        transform.LookAt(puntoAMirar);

        while(Vector3.Distance(transform.position, destino.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);
            yield return null;
        }

        transform.position = destino.position;
        moving = false;
    }
}
