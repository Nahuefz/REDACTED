using UnityEngine;

public class BasicItem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log($"Objeto {gameObject.name} pickeado!");
        Destroy(gameObject);
    }
}
