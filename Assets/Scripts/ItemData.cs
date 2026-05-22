using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public ItemType itemType = ItemType.Default;
    public string itemDescription;
    [Range(1, 10)]public int maxItemQuantity = 1; // implementar mas tarde
}

public enum ItemType
{
    ItemRata,
    Default
}
