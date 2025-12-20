using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Inventory playerInventory = new Inventory(99,9999);
    public Action onPlayerInventoryChanged;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool AddItemToInventory(Item item, int amount)
    {
        bool m = playerInventory.Add(item, amount);
        onPlayerInventoryChanged?.Invoke();
        return m;
    }

    public Item RemoveItem(Item item, int amount)
    {
        Item _item = playerInventory.Remove(item, amount);
        onPlayerInventoryChanged?.Invoke();
        return _item;
    }

    public Inventory GetPlayerInventory()
        => playerInventory;
}