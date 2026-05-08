using UnityEngine;

public class DialogosCindy : MonoBehaviour, IInteractable, IInterceptor
{

    [Header("Interaccion Directa")]
    public DialogoData[] dialogosDirectos;
    private int indiceDirecto = 0;

    [Header("Interaccion Indirecta")]
    public DialogoData[] dialogosIndirectos;
    private int indiceIndirecto = 0;
    public Transform puntoDeAparicion;

    [Header("Componentes y audio")]
    public GameObject iconoInteraccion;
    public AudioSource audioSource;
    public AudioClip[] sonidosInteraccion;

    private RegresoSigiloso comportamientoRegreso;

  

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        iconoInteraccion.SetActive(false);

        comportamientoRegreso = GetComponent<RegresoSigiloso>();
    }

    public void Interact()
    {
        if (DialogueManager.Instance.EstaHablando())
        {
            DialogueManager.Instance.MostrarSiguienteOracion();
            return;
        }

        ReproducirSonidoRandom();

        if (dialogosDirectos.Length > 0)
        {
            DialogueManager.Instance.EmpezarDialogo(dialogosDirectos[indiceDirecto]);

            if (indiceDirecto < dialogosDirectos.Length - 1)
            {
                indiceDirecto++;
                indiceIndirecto++;
            }
        }

    }

    public void InterceptPlayer (Transform player)
    {
        if (DialogueManager.Instance.EstaHablando()) return;

        if (puntoDeAparicion != null)
        {
            transform.position = puntoDeAparicion.position;
        }

        Vector3 puntoAMirar = new Vector3(transform.position.x, player.position.y, transform.position.z);
        player.LookAt(puntoAMirar);
        PlayerController playercontroller = player.GetComponent<PlayerController>();

        if (playercontroller != null)
        {
            playercontroller.LookAtFront();
        }

        ReproducirSonidoRandom();

        if (dialogosIndirectos.Length > 0)
        {
            DialogueManager.Instance.EmpezarDialogo(dialogosIndirectos[indiceIndirecto]);

            if(indiceIndirecto < dialogosIndirectos.Length - 1)
            {
                indiceIndirecto++;
                indiceDirecto++;
            }

            if (comportamientoRegreso != null)
            {
                comportamientoRegreso.IniciarRegreso();
            }
        }

    }

    private void ReproducirSonidoRandom()
    {
        if(audioSource != null && sonidosInteraccion.Length > 0)
        {
            int indice = Random.Range(0, sonidosInteraccion.Length);
            audioSource.clip = sonidosInteraccion[indice];
            audioSource.Play();
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) iconoInteraccion.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) iconoInteraccion.SetActive(false);
    }
}
