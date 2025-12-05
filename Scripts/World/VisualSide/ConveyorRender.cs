using System.Collections.Generic;
using UnityEngine;

public class ConveyorRender : MonoBehaviour
{
    // Clave única: combinación de conveyor + slot
    private class ItemSlot
    {
        public ConveyorLogic conveyor;
        public int slotIndex;

        public override bool Equals(object obj)
        {
            if (obj is ItemSlot other)
                return conveyor == other.conveyor && slotIndex == other.slotIndex;
            return false;
        }

        public override int GetHashCode()
        {
            return conveyor.GetHashCode() ^ slotIndex;
        }
    }

    private Dictionary<ItemSlot, GameObject> activeItems = new Dictionary<ItemSlot, GameObject>();
    public GameObject itemPrefab;
    public int poolSize = 50;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(itemPrefab);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    private void Start()
    {
        LogicManager.Instance.OnTick += RenderConveyorItems;
    }

    void RenderConveyorItems()
    {
        ConveyorLogic[] conveyorBlocks = LogicManager.Instance.GetLogicByType<ConveyorLogic>();
        HashSet<ItemSlot> stillActive = new HashSet<ItemSlot>();

        for (int i = 0; i < conveyorBlocks.Length; i++)
        {
            var conveyor = conveyorBlocks[i];
            
            // Fwd and right
            Vector2Int fwd = conveyor.ForwardFromRotation(conveyor.building.rotation);
            Vector2Int right = new Vector2Int(-fwd.y, fwd.x);
            
            Vector3 basePos = new Vector3(conveyor.building.position.x + 0.5f, conveyor.building.position.y + 0.5f, 0);

            for (int j = 0; j < conveyor.itemBuffer.Length; j++)
            {
                Item bufferItem = conveyor.itemBuffer[j];
                if (bufferItem == null) continue;

                ItemSlot slot = new ItemSlot { conveyor = conveyor, slotIndex = j };
                stillActive.Add(slot);

                if (!activeItems.TryGetValue(slot, out GameObject visual))
                {
                    visual = pool.Count > 0 ? pool.Dequeue() : Instantiate(itemPrefab);
                    visual.SetActive(true);
                    activeItems[slot] = visual;
                }

                visual.GetComponent<SpriteRenderer>().sprite = bufferItem.icon;
                float offset = (1f / ConveyorLogic.SLOT_COUNT);
                
                // Posición a lo largo del conveyor según el slot y la rotacion
                Vector3 slotOffset = new Vector3(fwd.x, fwd.y, 0) * (j * offset);
                visual.transform.position = basePos + slotOffset;
            }
        }

        List<ItemSlot> toRemove = new List<ItemSlot>();
        foreach (var kvp in activeItems)
        {
            if (!stillActive.Contains(kvp.Key))
            {
                kvp.Value.SetActive(false);
                pool.Enqueue(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }
        foreach (var slot in toRemove) activeItems.Remove(slot);
    }
}