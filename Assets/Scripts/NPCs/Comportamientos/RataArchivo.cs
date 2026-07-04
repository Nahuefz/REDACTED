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

    [Header("Conexion con el Dialogo")]
    public OficinistaNPC npcScript;
    private bool habilitarInteraccion = false;
    public MissionNames misionRata;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent != null)
        {
            _agent.speed = velocidad;
        }

        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (npcScript == null) npcScript = GetComponent<OficinistaNPC>();
        if (npcScript != null)
        {
            if (!GlobalMissions.GetMission(misionRata.ToString())) return;//si da false, todavia no se cumplio la mision
            npcScript.interaccionHabilitada = false;
        }
    }

    public void EscapeTo(Transform nuevoPunto, bool esPuntoFinal = false)
    {
        if (nuevoPunto == null || _agent == null) return;

        habilitarInteraccion = esPuntoFinal;
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

        if (habilitarInteraccion)
        {
            if (npcScript != null) npcScript.interaccionHabilitada = true;
            habilitarInteraccion = false;
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
