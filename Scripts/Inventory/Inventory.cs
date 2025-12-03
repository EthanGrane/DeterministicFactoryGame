using UnityEngine;

public class Inventory
{
    public InventorySlot[] slots;

    public Inventory(int slotCount)
    {
        slots = new InventorySlot[slotCount];
    }

    // Agregar items al inventario
    public bool Add(Item item, int amount)
    {
        foreach (var slot in slots)
        {
            if(slot.item == item && !slot.IsFull)
            {
                int space = slot.capacity - slot.amount;
                int toAdd = Mathf.Min(space, amount);
                slot.amount += toAdd;
                amount -= toAdd;
                if(amount <= 0) return true;
            }
        }
        return amount <= 0;
    }

    // Sacar items del inventario
    public bool Remove(Item item, int amount)
    {
        foreach(var slot in slots)
        {
            if(slot.item == item && slot.amount > 0)
            {
                int toRemove = Mathf.Min(slot.amount, amount);
                slot.amount -= toRemove;
                amount -= toRemove;
                if(amount <= 0) return true;
            }
        }
        return amount <= 0;
    }
}