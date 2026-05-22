using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class AskItem : MonoBehaviour, IInteractable
{
    [Header("Items")]
    [SerializeField] private ItemData goalItem; //?
    [SerializeField] private ItemType interactionWithItem;
    //agregar mas parametros a gusto
    public DialogoData AskDialogue;
    public DialogoData ContinueDialogue;

    public GameObject BlockGameObject;
    public bool isReceived = false;

    private Inventory _cachedInventory;
    //
    public void Interact(GameObject interactor)
    {
        if (DialogueManager.Instance.EstaHablando())
        {
            DialogueManager.Instance.MostrarSiguienteOracion();
            return;
        }

        _cachedInventory = interactor.GetComponent<Inventory>();

        if (isReceived)
        {
            if (ContinueDialogue != null)
            {
                DialogueManager.Instance.EmpezarDialogo(ContinueDialogue, _cachedInventory);
            }
            return;
        }

        ItemData requiredItem = GetRequiredItem();
        if (requiredItem != null)
        {
            EndInteraction(requiredItem);
        }
        else
        {
            if (AskDialogue != null)
            {
                DialogueManager.Instance.EmpezarDialogo(AskDialogue, _cachedInventory);
            }
        }
    }

    private ItemData GetRequiredItem()
    {
        if (_cachedInventory == null) return null;
        foreach (var item in _cachedInventory.GetInventory)
        {
            if (item == null) continue;

            if (goalItem != null)
            {
                if (item == goalItem) return item;
            }
            else if (item.itemType == interactionWithItem && interactionWithItem != ItemType.Default)
            {
                return item;
            }
        }
        return null;
    }

    // Helper method to maintain compatibility with existing check if needed
    private bool HasRequiredItem() => GetRequiredItem() != null;

    void EndInteraction(ItemData itemToRemove)
    {
        isReceived = true;
        
        if (itemToRemove != null)
        {
            _cachedInventory.RemoveItem(itemToRemove);
        }
        
        if (ContinueDialogue != null)
        {
            DialogueManager.Instance.EmpezarDialogo(ContinueDialogue, _cachedInventory);
        }

        if (BlockGameObject != null)
        {
            BlockGameObject.SetActive(false);
        }

        Debug.Log((itemToRemove != null ? itemToRemove.itemName : "Item") + " has been received and the path is clear");
    }
}
