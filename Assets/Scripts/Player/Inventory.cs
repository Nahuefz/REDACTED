using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> inventory = new List<GameObject>();
    private int _maxSize = 5; 

    void TryAddItem(GameObject item)
    {
        if (inventory.Count < 5 && !inventory.Contains(item)) inventory.Add(item);
        else Debug.Log("INVENTARIO LLENO");
        //llamado al delegate para que actualize LA FUTURA ui del inventario
    }
}
