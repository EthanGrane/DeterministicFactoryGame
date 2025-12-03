using UnityEngine;

[CreateAssetMenu(fileName = "New Storage Block", menuName = "FACTORY/Block/Storage Block")]
public class StorageBlock : Block<StorageLogic>
{
    [Header("Storage Block")]
    public int slotCount = 1;
}
