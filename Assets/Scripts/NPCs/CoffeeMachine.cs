using UnityEngine;

public class CoffeeMachine : MonoBehaviour, IInteractable
{
    [SerializeField] OutlineTesting outline;

    public void Interact()
    {
        Debug.Log(" INTERACCION Coffee Machine");
    }
}
