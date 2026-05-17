using UnityEngine;

public class OficinistaNPC : MonoBehaviour, IInteractable
{

    [Header("Datos")]
    public DialogoData[] listaDialogos;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] sonidosInteraccion;

    [Header("Ancla Cinematografica")]
    public Transform anclaDeInteraccion;
    private bool seEstaAlineando = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (DialogueManager.Instance.EstaHablando())
        {
            DialogueManager.Instance.MostrarSiguienteOracion();
            return;
        }
        
        if (seEstaAlineando) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && anclaDeInteraccion != null)
        {
            PlayerAlignment alignment = player.GetComponent<PlayerAlignment>();
            if (alignment != null)
            {
                seEstaAlineando = true;
                alignment.Alinear(anclaDeInteraccion, transform, () =>
                {
                    seEstaAlineando = false;
                    LanzarDialogo();
                });
            }
        }
        else
        {
            Debug.LogWarning("Te olvidaste de asignar el ancla!");
            LanzarDialogo();
        }
    }

    private void LanzarDialogo()
    {
        ReproducirSonidoRandom();
        if (listaDialogos != null && listaDialogos.Length > 0)
        {
            int indiceDialogo = Random.Range(0, listaDialogos.Length);
            DialogueManager.Instance.EmpezarDialogo(listaDialogos[indiceDialogo]);
        }
        else
        {
            Debug.LogWarning("¡El NPC " + gameObject.name + " no tiene diálogos asignados!");
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
