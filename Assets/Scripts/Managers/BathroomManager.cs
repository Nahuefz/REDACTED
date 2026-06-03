using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BathroomManager : MonoBehaviour
{
    [Header("Referencias de actores")]
    [SerializeField] private DialogosPereyra pereyraRef;
    [SerializeField] private RataBanio comportamientoRataRef;

    [Header("Controladores")]
    [SerializeField] private MonoBehaviour playerInputScript;
    [SerializeField] private GameObject playerObj;

    private bool _eventoIniciado = false;

    public void TriggerEventoRataBanio()
    {
        if (_eventoIniciado) return;
        _eventoIniciado = true;

        StartCoroutine(SequenceRoutine());
    }   
    private IEnumerator SequenceRoutine()
    {
        if (playerInputScript != null) playerInputScript.enabled = false;

        if(comportamientoRataRef != null)
        {
            comportamientoRataRef.IniciarFadeOut();
        }
        if (pereyraRef != null)
        {
            pereyraRef.gameObject.SetActive(true);
            pereyraRef.IniciarEscenaBanio(playerObj);
        }

        yield return new WaitUntil(() => !DialogueManager.Instance.EstaHablando());

        if(playerInputScript != null) playerInputScript.enabled = true;

        Debug.Log("Secuencia del baño terminada con exito");
    }     
}

