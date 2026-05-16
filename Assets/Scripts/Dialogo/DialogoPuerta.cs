using UnityEngine;
using System.Collections;

public class DialogoPuerta : MonoBehaviour, IInteractable
{
    [Header("Dialogo de la Puerta")]
    public DialogoData dialogoPuerta;

    [Header("Conexiones")]
    public DialogosCindy npc;
    public AudioSource audioPuerta;

    public void Interact()
    {
        if (DialogueManager.Instance.EstaHablando()) return;

        StartCoroutine(RutinaPuerta());
    }

    private IEnumerator RutinaPuerta()
    {
        if (audioPuerta != null) audioPuerta.Play();
        if (dialogoPuerta !=null)
        {
            DialogueManager.Instance.EmpezarDialogo(dialogoPuerta);
        }

        //Para esperar el click
        yield return new WaitWhile(() => DialogueManager.Instance.EstaHablando());

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
