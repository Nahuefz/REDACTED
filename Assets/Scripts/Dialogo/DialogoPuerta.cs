using UnityEngine;
using System.Collections;

public class DialogoPuerta : MonoBehaviour, IInteractable
{
    [Header("Dialogo de la Puerta")]
    public DialogoData dialogoPuerta;

    [Header("Cindy")]
    public DialogosCindy npc;

    [Header("El Jefe")]
    public DialogoData dialogoJefe;
    public ActorDialogo[] elJefe;
    public AudioSource audioSource;
    public AudioClip[] sonidosInteraccion;

    public void Interact()
    {
        if (DialogueManager.Instance.EstaHablando()) return;

        StartCoroutine(RutinaPuerta());
    }

    private IEnumerator RutinaPuerta()
    {
        bool yaHablamosConCindy = (npc != null && npc.YaHabloConElJugador());

        if (!yaHablamosConCindy)
        {
            if (dialogoPuerta != null)
            {
                DialogueManager.Instance.EmpezarDialogo(dialogoPuerta);
                //Para esperar el click
                yield return new WaitWhile(() => DialogueManager.Instance.EstaHablando());
            }
            //Interaccion con Cindy
            if (npc != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    IInterceptor interceptor = npc.GetComponent<IInterceptor>();
                    if (interceptor != null)
                    {
                        interceptor.InterceptPlayer(player.transform);
                    }
                }
            }
        }
        else
        {
            ReproducirSonidoRandom();
            //El Jefe
            if (dialogoJefe != null) DialogueManager.Instance.EmpezarDialogo(dialogoJefe, elJefe);
        }
    }

    private void ReproducirSonidoRandom()
    {
        if (audioSource != null && sonidosInteraccion.Length > 0)
        {
            int indice = Random.Range(0, sonidosInteraccion.Length);
            audioSource.clip = sonidosInteraccion[indice];
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entro");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Salio");
        }
    }

}
