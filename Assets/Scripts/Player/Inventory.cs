using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> inventory = new List<ItemData>();
    private int _maxSize = 5;

    //public event Action OnInventoryChanged;
    
    public bool TryAddItem(ItemData item)
    {
        if (inventory.Count < _maxSize && !inventory.Contains(item))
        {
            inventory.Add(item);
            Debug.Log("\"Objeto añadido: \" + item.itemName + \". Total: \" + inventory.Count");
            return true;
        }
        else if (inventory.Contains(item))
        {
            Debug.Log("item duplicado?");
            return false;
        }
        else
        {
            Debug.Log("INVENTARIO LLENO");
            return false;
        }
        //llamado al delegate para que actualize LA FUTURA ui del inventario
    }
}
