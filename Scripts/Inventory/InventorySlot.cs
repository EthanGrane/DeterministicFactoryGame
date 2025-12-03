using UnityEngine;

public class InventorySlot
{
    public Item item;
    public int amount;
    public int capacity;
    
    public bool IsFull => amount >= capacity;
}
