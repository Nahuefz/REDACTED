using UnityEngine;
using System.Collections;

public class DialogoPuerta : MonoBehaviour, IInteractable
{
    [Header("Dialogo de la Puerta")]
    public DialogoData dialogoPuerta;

    [Header("Conexiones")]
    public DialogosCindy npc;
    public GameObject iconoInteraccion;
    public AudioSource audioPuerta;


    void Start()
    {
        if (iconoInteraccion != null) iconoInteraccion.SetActive(false);
    }

    public void Interact()
    {
        if (DialogueManager.Instance.EstaHablando()) return;
        if (iconoInteraccion != null) iconoInteraccion.SetActive(false);

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
            if (iconoInteraccion != null) iconoInteraccion.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (iconoInteraccion != null) iconoInteraccion.SetActive(false);
        }
    }

}
