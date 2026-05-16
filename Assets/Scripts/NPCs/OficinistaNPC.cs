using UnityEngine;

public class OficinistaNPC : MonoBehaviour, IInteractable
{

    [Header("Datos")]
    public DialogoData[] listaDialogos;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] sonidosInteraccion;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (DialogueManager.Instance.EstaHablando())
        {
            DialogueManager.Instance.MostrarSiguienteOracion();
        }
        else
        {
            ReproducirSonidoRandom();
            if(listaDialogos != null && listaDialogos.Length > 0)
            {
                int indiceDialogo = Random.Range(0, listaDialogos.Length);
                DialogueManager.Instance.EmpezarDialogo(listaDialogos[indiceDialogo]);
            }
            else
            {
                Debug.LogWarning("¡El NPC " + gameObject.name + " no tiene diálogos asignados!");
            }
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
