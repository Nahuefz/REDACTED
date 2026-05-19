using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerInputs inputActions;

    private IInteractable interactuableCercano;

    private InteractRay _interactRay;


    private void Awake()
    {
        inputActions = new PlayerInputs();
        _interactRay = GetComponent<InteractRay>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        Debug.Log("Enable");
        inputActions.Enable();
        inputActions.Player.Interact.performed += EjecutarInteraccion;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= EjecutarInteraccion;
        inputActions.Disable();
        Debug.Log("Disable");
    }


    private void EjecutarInteraccion(InputAction.CallbackContext context)
    {
        if (interactuableCercano != null)
        {
            Debug.Log("Interactua");
            interactuableCercano.Interact(this.gameObject);
        }
        //Debug.Log("FUNCIONA EL INPUT");
        _interactRay.CurrentInteractable?.Interact(this.gameObject);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactuable = other.GetComponent<IInteractable>();
        if (interactuable != null)
        {
            interactuableCercano = interactuable;
            Debug.Log("Jugador detect� a un NPC: " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactuableCercano = null;
            Debug.Log("Jugador se alej� del NPC");
        }
    }

}
