using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference accionInteractuar;
    
    private IInteractable interactuableCercano;

    private void OnEnable()
    {
        accionInteractuar.action.performed += EjecutarInteraccion;
    }

    private void OnDisable()
    {
        accionInteractuar.action.performed -= EjecutarInteraccion;
    }


    private void EjecutarInteraccion(InputAction.CallbackContext context)
    {
        if (interactuableCercano != null)
        {
            Debug.Log("Interactua");
            interactuableCercano.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactuable = other.GetComponent<IInteractable>();
        if (interactuable != null)
        {
            interactuableCercano = interactuable;
            Debug.Log("Jugador detectó a un NPC: " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactuableCercano = null;
            Debug.Log("Jugador se alejó del NPC");
        }
    }

}
