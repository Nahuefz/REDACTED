using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BathroomManager : MonoBehaviour
{
    [Header("Referencias de actores")]
    [SerializeField] private DialogosPereyra pereyraRef;
    [SerializeField] private ComportamientoRata comportamientoRataRef;
    [Header("Referencias de ubicación")]
    [SerializeField] private Transform pereyraPoint;

    [Header("Controladores")]
    [SerializeField] private MonoBehaviour playerInputScript;
    [SerializeField] private GameObject playerObj;

    private bool _eventoIniciado = false;

    public void TriggerEventoRataBanio()
    {
        if (_eventoIniciado)
        {
            Debug.Log("El evento ya se inició, no se puede repetir.");
            return;
        }
        _eventoIniciado = true;
        Debug.Log("El Manager recibió la orden y empieza la secuencia.");
        StartCoroutine(SequenceRoutine());
    }
    private IEnumerator SequenceRoutine()
    {
        // 1. Bloqueo de input
        if (playerInputScript != null) playerInputScript.enabled = false;

        // 2. Fade de la rata
        if (comportamientoRataRef != null)
            comportamientoRataRef.IniciarFadeOut();

        // 3. Activar y posicionar Pereyra SIN alinear al jugador inmediatamente
        if (pereyraRef != null && pereyraPoint != null)
        {
            pereyraRef.gameObject.SetActive(true);
            pereyraRef.transform.position = pereyraPoint.position;
            pereyraRef.transform.rotation = pereyraPoint.rotation;

            // IMPORTANTE: Si IniciarEscenaBanio() mueve al jugador, 
            // crea una variante que solo dispare el diálogo.
            pereyraRef.IniciarEscenaBanio(playerObj);
        }

        yield return new WaitUntil(() => DialogueManager.Instance != null && !DialogueManager.Instance.EstaHablando());

        if (playerInputScript != null) playerInputScript.enabled = true;
    }
}



