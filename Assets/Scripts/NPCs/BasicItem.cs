using UnityEngine;
public class BasicItem : MonoBehaviour, IInteractable
{
    //[SerializeField]private Inventory _playerInvetory;
    public ItemData data;

    private void Start()
    {
        //_playerInvetory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (data == null) Debug.Log("FALTA ITEMDATA");
    }

    public void Interact(GameObject interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();
        if (inventory != null && inventory.TryAddItem(data))
        {
            Debug.Log($"Objeto <color=yellow>{data.itemName}</color> pickeado!");
            Destroy(gameObject);
            return;
        }
        Debug.Log("inventario lleno");
    }
}
