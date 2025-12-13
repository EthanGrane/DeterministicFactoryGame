using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public EnemyTierSO currentTierSo;
    public float moveSpeed = 2f;
    public float collisionRadius = 0.5f;
    private Vector2Int currentTarget;

    private bool isAlive = true;

    // Path
    private List<Vector2> path;
    private int currentNodeIndex = 0;

    // Movimiento
    private Vector2 moveDirection;

    private const float NODE_REACHED_DIST = 0.1f;

    private void Start()
    {
        EnemyManager.Instance.ApplyTier(this, currentTierSo);
        LogicManager.Instance.OnTick += RecalculatePath;
    }

    private void OnDestroy()
    {
        if (LogicManager.Instance != null)
            LogicManager.Instance.OnTick -= RecalculatePath;
    }

    private void Update()
    {
        if (!isAlive || path == null || currentNodeIndex >= path.Count)
            return;

        Vector2 currentPos = transform.position;
        Vector2 target = path[currentNodeIndex];
        Vector2 toTarget = target - currentPos;

        if (toTarget.sqrMagnitude <= NODE_REACHED_DIST * NODE_REACHED_DIST)
        {
            currentNodeIndex++;
            return;
        }

        moveDirection = toTarget.normalized;
        Vector2 delta = moveDirection * (moveSpeed * Time.deltaTime);

        if (delta.sqrMagnitude > toTarget.sqrMagnitude)
            delta = toTarget;

        transform.Translate(delta, Space.World);
        
        
        if (Vector2.Distance(transform.position, Pathfinding.Instance.GetTargetNode().Value) <= 1f)
        {
            Debug.Log("Reached the end of the path");
            TakeDamage(999);
        }
    }

    private void RecalculatePath()
    {
        if (!isAlive) return;

        if (path != null && currentNodeIndex < path.Count)
            return;

        Vector2Int start = WorldPosToGrid(transform.position);
        Vector2Int? end = Pathfinding.Instance.GetTargetNode();

        currentTarget = end.Value;

        List<Vector2Int> gridPath =
            Pathfinding.Instance.FindPath(start, end.Value);

        if (gridPath == null || gridPath.Count == 0)
            return;

        path = new List<Vector2>(gridPath.Count);
        foreach (var p in gridPath)
            path.Add(GridToWorld(p));

        currentNodeIndex = 0;
        
    }
    
    private Vector2Int WorldPosToGrid(Vector2 pos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(pos.x),
            Mathf.FloorToInt(pos.y)
        );
    }

    private Vector2 GridToWorld(Vector2Int grid)
    {
        return new Vector2(
            grid.x + 0.5f,
            grid.y + 0.5f
        );
    }

    public void TakeDamage(int dmg)
    {
        EnemyManager.Instance.ProcessDamage(this, dmg);
    }
    
    public void DieExtern()
    {
        if (!isAlive) return;

        isAlive = false;
        moveDirection = Vector2.zero;
        Destroy(gameObject);
    }

    public Vector2 GetPosition() => transform.position;
    public Vector2 GetVelocity() => moveDirection * moveSpeed;
}
