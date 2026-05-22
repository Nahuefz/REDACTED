using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // [SerializeField] private List<ItemData> inventory = new List<ItemData>();
    // private int _maxSize = 5;
    [SerializeField] private InventoryINFO _inventoryInfo;

    public event Action OnInventoryChanged;
    private static bool _yaInicializado = false;

    private void Start()
    {
        if(!_yaInicializado)
        {
            _inventoryInfo.Clear();
            _yaInicializado = true;
        }
    }

    public bool TryAddItem(ItemData item)
    {
        if (_inventoryInfo.list.Count < _inventoryInfo.maxSize)
        {
            _inventoryInfo.AddItem(item);
            OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }
    public List<ItemData> GetInventory => _inventoryInfo.list;
    // #region MyRegion
    //
    // public bool TryAddItem(ItemData item)
    // {
    //     if (inventory.Count < _maxSize && !inventory.Contains(item))
    //     {
    //         inventory.Add(item);
    //         //Debug.Log("\"Objeto añadido: \" + item.itemName + \". Total: \" + inventory.Count");
    //         OnInventoryChanged?.Invoke();
    //         return true;
    //     }
    //     else if (inventory.Contains(item) && inventory.Count < _maxSize)
    //     {
    //         Debug.Log("item duplicado?");
    //         return false;
    //     }
    //     else
    //     {
    //         Debug.Log("INVENTARIO LLENO");
    //         return false;
    //     }
    //     //llamado al delegate para que actualize LA FUTURA ui del inventario
    // }
    //
    // public List<ItemData> GetInventory()
    // {
    //     return inventory;
    // }
    //
    // #endregion
    
}
