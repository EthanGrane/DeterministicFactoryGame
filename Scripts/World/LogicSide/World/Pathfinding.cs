using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    [Header("Pathfinding Settings")]
    public bool canBreakObstacles = true;
    public float obstacleBreakCostMultiplier = 1f;

    [Header("Movement Costs")]
    public float straightCost = 1f;

    private Vector2Int? start = null;
    private Vector2Int? end = null;

    public FlowField CurrentFlow { get; private set; }

    // Solo movimientos ortogonales
    private static readonly Vector2Int[] Neighbors =
    {
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0),
        new Vector2Int(1,0)
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /* =========================
     * PUBLIC API
     * ========================= */
    public FlowField BuildFlowField()
    {
        EnsureEndpoints();

        int size = World.WorldSize;
        Tile[,] tiles = World.Instance.GetTiles();
        if (tiles == null) return null;

        FlowField field = new FlowField(size);
        PriorityQueue<Vector2Int> open = new PriorityQueue<Vector2Int>();
        Vector2Int goal = end.Value;

        field.cost[goal.x, goal.y] = 0f;
        open.Enqueue(goal, 0f);

        // Costes progresivos para evitar que queden pegados a muros
        float[] wallCosts = { 1000f, 5f, 2f, 1f, 0f };

        while (open.Count > 0)
        {
            Vector2Int current = open.Dequeue();
            float currentCost = field.cost[current.x, current.y];

            foreach (Vector2Int dir in Neighbors)
            {
                Vector2Int n = current + dir;
                if (!IsValid(n)) continue;

                Tile tile = tiles[n.x, n.y];
                if (tile == null || tile.terrainSO == null || tile.terrainSO.solid)
                    continue;

                float moveCost = tile.terrainSO.movementCost * straightCost;

                // Coste por proximidad a muros
                byte dist = Pathfinding.Instance.wallDistance != null ? Pathfinding.Instance.wallDistance[n.x, n.y] : (byte)5;
                moveCost += dist < wallCosts.Length ? wallCosts[dist] : 0f;

                // Obstáculo rompible
                if (tile.building?.block != null && tile.building.block.solid)
                {
                    if (!canBreakObstacles) continue;
                    moveCost += tile.building.block.blockHealth * obstacleBreakCostMultiplier;
                }

                float newCost = currentCost + moveCost;

                if (newCost < field.cost[n.x, n.y])
                {
                    field.cost[n.x, n.y] = newCost;
                    open.Enqueue(n, newCost);
                }
            }
        }

        BuildDirectionField(field);
        CurrentFlow = field;
        return field;
    }

    public Vector2 GetDirection(Vector2 worldPos)
    {
        if (CurrentFlow == null) return Vector2.zero;

        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.y);

        if (!IsValid(new Vector2Int(x, y))) return Vector2.zero;

        return CurrentFlow.direction[x, y];
    }

    public void SetStart(Vector2Int p) => start = p;
    public void SetEnd(Vector2Int p) => end = p;

    /* =========================
     * INTERNAL
     * ========================= */
    private void BuildDirectionField(FlowField field)
    {
        for (int x = 0; x < field.size; x++)
        for (int y = 0; y < field.size; y++)
        {
            float best = field.cost[x, y];
            Vector2 bestDir = Vector2.zero;

            foreach (Vector2Int dir in Neighbors)
            {
                Vector2Int n = new Vector2Int(x + dir.x, y + dir.y);
                if (!IsValid(n)) continue;

                float c = field.cost[n.x, n.y];
                if (c < best)
                {
                    best = c;
                    bestDir = new Vector2(dir.x, dir.y).normalized;
                }
            }

            field.direction[x, y] = bestDir;
        }
    }

    private void EnsureEndpoints()
    {
        if (!start.HasValue)
        {
            var spawn = FindFirstObjectByType<EnemySpawnPoint>();
            if (spawn != null) start = new Vector2Int((int)spawn.transform.position.x, (int)spawn.transform.position.y);
        }
        if (!end.HasValue)
        {
            var basePoint = FindFirstObjectByType<PlayerBasePoint>();
            if (basePoint != null) end = new Vector2Int((int)basePoint.transform.position.x, (int)basePoint.transform.position.y);
        }
    }

    private bool IsValid(Vector2Int p) => p.x >= 0 && p.x < World.WorldSize && p.y >= 0 && p.y < World.WorldSize;

    public byte[,] wallDistance; // Expuesto para costes progresivos
}

public class FlowField
{
    public readonly int size;
    public readonly float[,] cost;
    public readonly Vector2[,] direction;

    public FlowField(int size)
    {
        this.size = size;
        cost = new float[size, size];
        direction = new Vector2[size, size];

        for (int x = 0; x < size; x++)
        for (int y = 0; y < size; y++)
            cost[x, y] = float.MaxValue;
    }
}

public class PriorityQueue<T>
{
    private readonly List<(T item, float priority)> elements = new();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority) => elements.Add((item, priority));

    public T Dequeue()
    {
        int bestIndex = 0;
        float bestPriority = elements[0].priority;

        for (int i = 1; i < elements.Count; i++)
        {
            if (elements[i].priority < bestPriority)
            {
                bestPriority = elements[i].priority;
                bestIndex = i;
            }
        }

        T item = elements[bestIndex].item;
        elements.RemoveAt(bestIndex);
        return item;
    }
}
