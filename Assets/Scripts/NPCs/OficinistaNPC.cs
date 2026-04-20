using UnityEngine;

public class OficinistaNPC : MonoBehaviour, IInteractable
{

    [Header("Datos")]
    public DialogoData datosDialogo;

    [Header("Componentes")]
    public GameObject iconoInteraccion; //La "E"

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] sonidosInteraccion;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        iconoInteraccion.SetActive(false);
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
            DialogueManager.Instance.EmpezarDialogo(datosDialogo);
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
            iconoInteraccion.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            iconoInteraccion.SetActive(false);
        }
    }


}
