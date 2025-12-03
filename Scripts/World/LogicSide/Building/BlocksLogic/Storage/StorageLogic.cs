using UnityEngine;

public class StorageLogic : BuildingLogic
{
    public Inventory inventory;

    public override void Initialize(Block block)
    {
        var storageBlock = (StorageBlock)block;
        inventory = new Inventory(storageBlock.slotCount);
    }
}
