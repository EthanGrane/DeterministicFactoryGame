    using UnityEngine;

    public class ConveyorLogic : BuildingLogic
    {
        private int conveyorTickSpeed;
        
        private Item[] itemBuffer;
        private int ticks;  // Timer
        
        const int slotCount = 4;
        
        public override void Initialize(Block block)
        {
            var conveyor = (ConveyorBlock)block;
            conveyorTickSpeed = conveyor.conveyorTickSpeed;
            itemBuffer = new Item[slotCount];
        }

        public override void Tick()
        {
            ticks++;
            if (ticks < conveyorTickSpeed) return;
            
            ticks = 0;
            MoveItems();
            DrawDebug();
        }

        private void DrawDebug()
        {
            Vector3 basePos = new Vector3(building.position.x + 0.5f, building.position.y + 0.5f, 0);

            float offset = 0.25f;

            for (int i = 0; i < slotCount; i++)
            {
                Vector3 start = basePos + new Vector3(0,-(1/slotCount) - 0.5f + i * offset, 0);
                Vector3 end   = start + Vector3.up * 0.1f;

                Color c = itemBuffer[i] == null ? Color.red : Color.green;

                Debug.DrawLine(start, end, c, 1f, false);
            }
        }

        
        private void MoveItems()
        {
            for (int i = slotCount - 1; i > 0; i--)
            {
                if (itemBuffer[i] == null && itemBuffer[i - 1] != null)
                {
                    itemBuffer[i] = itemBuffer[i - 1];
                    itemBuffer[i - 1] = null;
                }
            }

            TryOutput();
        }

        private void TryOutput()
        {
            // TODO: mira el edificio conectado adelante
            // si tiene espacio -> pásale el item slots[slotCount-1]
        }
        
        public bool TryInsert(Item item)
        {
            if (itemBuffer[0] == null)
            {
                itemBuffer[0] = item;
                return true;
            }
            return false;
        }
    }   
