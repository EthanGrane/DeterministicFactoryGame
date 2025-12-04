using UnityEngine;

public class StorageLogic : BuildingLogic, IItemAcceptor
{
    public Inventory inventory;

    public override void Initialize(Block block)
    {
        var storageBlock = (StorageBlock)block;
        inventory = new Inventory(storageBlock.slotCount);
    }

    public bool CanAccept(Item item)
    {
        return inventory.Add(item, 0);
    }

    public bool Insert(Item item)
    {
        return inventory.Add(item, 1);
    }
}
