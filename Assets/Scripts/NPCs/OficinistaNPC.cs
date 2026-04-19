using UnityEngine;

public class OficinistaNPC : MonoBehaviour, IInteractable
{

    [Header("Datos")]
    public DialogoData datosDialogo;

    [Header("Componentes")]
    public GameObject iconoInteraccion; //La "E"
    private AudioSource audioSource;

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
            if(audioSource != null) audioSource.Play();
            DialogueManager.Instance.EmpezarDialogo(datosDialogo);
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
