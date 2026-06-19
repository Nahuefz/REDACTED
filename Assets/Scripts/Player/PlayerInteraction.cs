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
        IInteractable target = null;

        // Prioridad al interactuable detectado por Raycast (mirada directa)
        if (_interactRay != null && _interactRay.CurrentInteractable != null)
        {
            target = _interactRay.CurrentInteractable;
        }
        // Si no hay nada en la mirada, usamos el detectado por trigger (proximidad)
        else if (interactuableCercano != null)
        {
            target = interactuableCercano;
        }

        if (target != null)
        {
            Debug.Log("Interactua con: " + target.ToString());
            target.Interact(this.gameObject);
        }
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
