using System;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public static LogicManager Instance { get; private set; }

    private List<BuildingLogic> logics = new List<BuildingLogic>();

    public const int TICKS_PER_SECOND = 60;
    private float tickLength => 1f / TICKS_PER_SECOND;
    private float accumulator = 0;

    public Action OnTick;

    private void Awake()
    {
        Instance = this;
    }

    public void Register(BuildingLogic logic)
    {
        logics.Add(logic);
        logic.Initialize(logic.building.block);
    }

    public void Unregister(BuildingLogic logic)
    {
        logics.Remove(logic);
    }
    
    void Update()
    {
        accumulator += Time.deltaTime;

        while (accumulator >= tickLength)
        {
            accumulator -= tickLength;

            foreach (var logic in logics)
                if (logic.update)
                    logic.Tick();
            
            PushItemsToNeighbors();
            
            OnTick?.Invoke();
        }
    }

    public T[] GetLogicByType<T>() where T : BuildingLogic
    {
        List<T> filtered = new List<T>();
        foreach(var logic in logics)
            if (logic is T t) filtered.Add(t);
        return filtered.ToArray();
    }
    
    private void PushItemsToNeighbors()
    {
        foreach (var logic in logics)
        {
            if (logic is not IItemProvider provider)
                continue;

            if (logic is ConveyorLogic)
                continue;

            Item itemToPush = null;
            IItemAcceptor targetAcceptor = null;

            var perimeter = GetPerimeterNeighbors(
                logic.building.position,
                logic.building.block.size
            );

            foreach (var pos in perimeter)
            {
                var neighbor = World.Instance.GetBuilding(pos);
                if (neighbor == null) continue;

                if (neighbor.logic is IItemAcceptor acceptor)
                {
                    Item peekItem = provider.PeekFirst();
                    if (peekItem == null)
                        break;

                    if (acceptor.CanAccept(peekItem))
                    {
                        itemToPush = peekItem;
                        targetAcceptor = acceptor;
                        break;
                    }
                }
            }

            if (targetAcceptor != null && itemToPush != null)
            {
                provider.Extract(itemToPush);
                targetAcceptor.Insert(itemToPush);
            }
        }
    }

    public static List<Vector2Int> GetOccupiedTiles(Vector2Int center, int size)
    {
        List<Vector2Int> tiles = new();
        int half = size / 2;

        for (int x = -half; x <= half; x++)
        for (int y = -half; y <= half; y++)
            tiles.Add(new Vector2Int(center.x + x, center.y + y));

        return tiles;
    }
    public static HashSet<Vector2Int> GetPerimeterNeighbors(Vector2Int origin, int size)
    {
        HashSet<Vector2Int> result = new();

        int minX = origin.x;
        int maxX = origin.x + size - 1;
        int minY = origin.y;
        int maxY = origin.y + size - 1;

        // Arriba y abajo
        for (int x = minX; x <= maxX; x++)
        {
            result.Add(new Vector2Int(x, maxY + 1)); // arriba
            result.Add(new Vector2Int(x, minY - 1)); // abajo
        }

        // Izquierda y derecha
        for (int y = minY; y <= maxY; y++)
        {
            result.Add(new Vector2Int(minX - 1, y)); // izquierda
            result.Add(new Vector2Int(maxX + 1, y)); // derecha
        }

        return result;
    }


}
