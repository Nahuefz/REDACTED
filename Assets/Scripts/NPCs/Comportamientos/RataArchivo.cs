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
        
        // --- CORRECCIÓN DE LÓGICA DE INTERACCIÓN ---
        if (npcScript != null)
        {
            // Le preguntamos a GlobalMissions si la misión de la rata YA se completó
            bool misionCompletada = GlobalMissions.GetMission(misionRata.ToString());

            if (misionCompletada)
            {
                // Si la misión ya se hizo y la rata apareció en su nueva posición:
                npcScript.interaccionHabilitada = true; 
                npcScript.yaEntregado = true; // Asegura que el NPC sepa que ya entregó su misión y dé el diálogo final
            }
            else
            {
                // Si la misión NO se ha hecho, bloqueamos la interacción (o la manejas según tu flujo inicial)
                // Nota: Si el jugador debe hablarle antes de que escape, déjalo en true. 
                // Si solo habla al llegar a su destino, déjalo en false.
                npcScript.interaccionHabilitada = false; 
            }
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
            int indice = Random.Range(0, sonidoCorriendo.Length); // Corregido el nombre a tu array "sonidoCorriendo"
            audioSource.clip = sonidoCorriendo[indice];
            audioSource.Play();
        }
    }
}