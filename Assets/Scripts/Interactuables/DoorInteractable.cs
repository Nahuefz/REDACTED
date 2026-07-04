using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Componentes")]
    [SerializeField] private Animator puertaAnimator; // Arrastrá acá el Animator de la puerta

    private bool estaAbierta = false;

    void Start()
    {
        if (puertaAnimator == null)
        {
            puertaAnimator = GetComponent<Animator>();
        }
    }

    // Esta función la ejecuta el script de tus compañeros automáticamente
    public void Interact(GameObject player)
    {
        if (puertaAnimator != null)
        {
            // Invertimos el estado actual
            estaAbierta = !estaAbierta;

            // Le pasamos el dato al Animator Controller
            puertaAnimator.SetBool("EstaAbierta", estaAbierta);

            Debug.Log(estaAbierta ? "Baño: Abriendo puerta." : "Baño: Cerrando puerta.");
        }
    }
}