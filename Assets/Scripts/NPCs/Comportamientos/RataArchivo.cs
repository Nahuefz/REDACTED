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
    
    [Header("Persistencia de Posición Final")]
    [SerializeField] private Transform puntoFinal;
    // Nombre de la flag global exclusiva para recordar que la rata llegó al destino
    private string FlagLlegoAlFinal => misionRata.ToString() + "_LlegoAlFinal";

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent != null)
        {
            _agent.speed = velocidad;
        }

        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        if (npcScript == null) npcScript = GetComponent<OficinistaNPC>();
        
        // --- COMPROBACIÓN: ¿YA HABÍA LLEGADO AL FINAL ANTES? ---
        if (GlobalMissions.GetMission(FlagLlegoAlFinal))
        {
            if (puntoFinal != null)
            {
                // Si usas NavMeshAgent, debes apagarlo antes de teletransportar para que no ignore la posición
                if (_agent != null) _agent.enabled = false; 
                
                transform.position = puntoFinal.position;
                transform.rotation = puntoFinal.rotation;
                
                // Volvemos a prenderlo si necesitas que conserve sus físicas o IA ahí parado
                if (_agent != null) _agent.enabled = true; 
            }

            if (npcScript != null)
            {
                npcScript.interaccionHabilitada = true; 
                npcScript.yaEntregado = true;
            }
            
            // Ya está en su lugar final de forma persistente, terminamos el Start aquí.
            return; 
        }

        // --- LÓGICA NORMAL SI NO HA LLEGADO AL FINAL ---
        if (npcScript != null)
        {
            bool misionCompletada = GlobalMissions.GetMission(misionRata.ToString());

            if (misionCompletada)
            {
                npcScript.interaccionHabilitada = true; 
                npcScript.yaEntregado = true; 
            }
            else
            {
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
        if (_agent != null)
        {
            // Detectar de forma real si el NavMeshAgent está corriendo y llegó a su destino
            if (audioSource != null && audioSource.isPlaying)
            {
                if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    audioSource.Stop();

                    // SI ERA EL PUNTO FINAL Y YA LLEGÓ CORRIENDO:
                    // Guardamos de forma persistente que ya completó su recorrido
                    if (habilitarInteraccion) 
                    {
                        GlobalMissions.SetMission(FlagLlegoAlFinal, true);
                    }
                }
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