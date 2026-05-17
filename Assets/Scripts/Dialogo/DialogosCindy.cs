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
    public AudioSource audioSource;
    public AudioClip[] sonidosInteraccion;

    [Header("Ancla Cinematografica")]
    public Transform anclaDeInteraccion;
    public Transform anclaDeMirada;
    private bool seEstaAlineando = false;

    private RegresoSigiloso comportamientoRegreso;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        comportamientoRegreso = GetComponent<RegresoSigiloso>();
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
                alignment.Alinear(anclaDeInteraccion, transform, anclaDeMirada, () =>
                {
                    seEstaAlineando = false;
                    LanzarDialogoDirecto();
                });
            }
        }
        else
        {
            Debug.LogWarning("Te olvidaste de asignar el ancla!");
            LanzarDialogoDirecto();
        }
    }

    private void LanzarDialogoDirecto()
    {
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

    public void InterceptPlayer(Transform player)
    {
        if (DialogueManager.Instance.EstaHablando()) return;

        if (puntoDeAparicion != null)
        {
            transform.position = puntoDeAparicion.position;
        }

        PlayerAlignment alignment = player.GetComponent<PlayerAlignment>();

        if (alignment != null && anclaDeInteraccion != null)
        {
            alignment.Alinear(anclaDeInteraccion, transform, anclaDeMirada, () =>
            {
                LanzarDialogoIndirecto();
            });
        }
        else
        {
            LanzarDialogoIndirecto();
        }
    }

    private void LanzarDialogoIndirecto()
    {
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
        if (other.CompareTag("Player")) Debug.Log("Entro");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) Debug.Log("Salio");
    }
}
