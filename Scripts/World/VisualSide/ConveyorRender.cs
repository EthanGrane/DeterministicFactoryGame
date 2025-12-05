using System.Collections.Generic;
using UnityEngine;

public class ConveyorRender : MonoBehaviour
{
    // Identificador único para cada posición de item (conveyor específico + índice de slot)
    // Necesario porque múltiples conveyors pueden tener el mismo tipo de Item
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

    // Diccionario que mapea cada slot a su GameObject visual
    private Dictionary<ItemSlot, GameObject> activeItems = new Dictionary<ItemSlot, GameObject>();
    public GameObject itemPrefab; // Prefab con SpriteRenderer para mostrar items
    public int poolSize = 50;
    private Queue<GameObject> pool = new Queue<GameObject>(); // Pool de objetos reutilizables

    void Awake()
    {
        // Inicializar pool de GameObjects
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(itemPrefab);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    private void Start()
    {
        // Suscribirse al evento de tick para actualizar visuales cada frame lógico
        LogicManager.Instance.OnTick += RenderConveyorItems;
    }

    void RenderConveyorItems()
    {
        ConveyorLogic[] conveyorBlocks = LogicManager.Instance.GetLogicByType<ConveyorLogic>();
        HashSet<ItemSlot> stillActive = new HashSet<ItemSlot>(); // Slots que siguen activos este frame

        // Iterar por todos los conveyors del mundo
        for (int i = 0; i < conveyorBlocks.Length; i++)
        {
            var conveyor = conveyorBlocks[i];
            
            // Calcular vectores de dirección según la rotación del conveyor
            Vector2Int fwd = conveyor.ForwardFromRotation(conveyor.building.rotation);
            Vector2Int right = new Vector2Int(-fwd.y, fwd.x); // Perpendicular a forward
            
            // Posición central del tile del conveyor
            Vector3 basePos = new Vector3(conveyor.building.position.x + 0.5f, conveyor.building.position.y + 0.5f, 0);
            float slotDistance = 1f / ConveyorLogic.SLOT_COUNT; // Distancia entre cada slot

            // Iterar por cada slot del buffer de items
            for (int j = 0; j < conveyor.itemBuffer.Length; j++)
            {
                Item bufferItem = conveyor.itemBuffer[j];
                if (bufferItem == null) continue; // Slot vacío, siguiente

                // Crear identificador único para este slot
                ItemSlot slot = new ItemSlot { conveyor = conveyor, slotIndex = j };
                stillActive.Add(slot); // Marcar como activo

                // Obtener o crear el GameObject visual para este slot
                if (!activeItems.TryGetValue(slot, out GameObject visual))
                {
                    visual = pool.Count > 0 ? pool.Dequeue() : Instantiate(itemPrefab);
                    visual.SetActive(true);
                    activeItems[slot] = visual;
                }

                // Actualizar sprite según el item
                visual.GetComponent<SpriteRenderer>().sprite = bufferItem.icon;
                
                // Calcular posición interpolada del item según su progreso
                float progress = conveyor.GetItemProgressNormalized(j); // 0-1
                float slotStart = j * slotDistance; // Inicio del slot actual
                float slotEnd = (j + 1) * slotDistance; // Final del slot (inicio del siguiente)
                
                // Interpolar posición entre inicio y fin del slot
                float lerpedPosition = Mathf.Lerp(slotStart, slotEnd, progress);
                
                // Aplicar offset en dirección forward del conveyor
                Vector3 slotOffset = new Vector3(fwd.x, fwd.y, 0) * lerpedPosition;
                visual.transform.position = basePos + slotOffset;
            }
        }

        // Limpiar visuales de slots que ya no existen (items consumidos/movidos)
        List<ItemSlot> toRemove = new List<ItemSlot>();
        foreach (var kvp in activeItems)
        {
            if (!stillActive.Contains(kvp.Key))
            {
                kvp.Value.SetActive(false); // Ocultar
                pool.Enqueue(kvp.Value); // Devolver al pool
                toRemove.Add(kvp.Key);
            }
        }
        foreach (var slot in toRemove) activeItems.Remove(slot);
    }
}