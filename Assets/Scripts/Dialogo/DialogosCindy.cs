using UnityEngine;

public class DialogosCindy : MonoBehaviour, IInteractable
{

    [Header("Interaccion Directa")]
    public DialogoData[] dialogosDirectos;
    private int indiceDirecto = 0;

    [Header("Interaccion Indirecta")]
    public DialogoData[] dialogosIndirectos;
    private int indiceIndirecto = 0;

    [Header("Componentes y audio")]
    public GameObject iconoInteraccion;
    public AudioSource audioSource;
    public AudioClip[] sonidosInteraccion;

    [Header("Configuracion de teletransporte")]
    public Transform puntoDeAparicion;

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

    public void InterceptarJugador(Transform jugador)
    {
        if (DialogueManager.Instance.EstaHablando()) return;

        if (puntoDeAparicion != null)
        {
            transform.position = puntoDeAparicion.position;
        }

        Vector3 puntoAMirar = new Vector3(transform.position.x, jugador.position.y, transform.position.z);
        jugador.LookAt(puntoAMirar);

        ReproducirSonidoRandom();

        if (dialogosIndirectos.Length > 0)
        {
            DialogueManager.Instance.EmpezarDialogo(dialogosIndirectos[indiceIndirecto]);

            if(indiceIndirecto < dialogosIndirectos.Length - 1)
            {
                indiceIndirecto++;
                indiceDirecto++;
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

}
