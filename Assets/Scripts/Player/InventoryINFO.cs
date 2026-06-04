using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryINFO", menuName = "Scriptable Objects/InventoryINFO")]
public class InventoryINFO : ScriptableObject
{
    public List<ItemData> list = new List<ItemData>();
    public int maxSize = 5;

    public void AddItem(ItemData item)
    {
        if (list.Count < maxSize && !list.Contains(item))
        {
            list.Add(item);
        }
    }
    
    public void Clear() => list.Clear();
}
