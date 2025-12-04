using UnityEngine;

public interface IItemAcceptor
{
    bool CanAccept(Item item);
    bool Insert(Item item);
}
