using UnityEngine;
using UnityEngine.AI;

public class RataArchivo : MonoBehaviour
{
    [Header("Configuracion de Movimiento")]
    private NavMeshAgent _agent;
    public float velocidad = 8f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] sonidoCorriendo;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent != null)
        {
            _agent.speed = velocidad;
        }

        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void EscapeTo(Transform nuevoPunto)
    {
        if (nuevoPunto == null || _agent == null) return;

        PlayRunningSound();
        _agent.SetDestination(nuevoPunto.position);
    }

    private void Update()
    {
        if (_agent != null && audioSource != null && audioSource.isPlaying)
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                audioSource.Stop();
            }
        }
    }

    private void PlayRunningSound()
    {
        if (audioSource != null && sonidoCorriendo.Length > 0 && !audioSource.isPlaying)
        {
            int indice = Random.Range(0, sonidoCorriendo.Length);
            audioSource.clip = sonidoCorriendo[indice];
            audioSource.Play();
        }
    }
}
