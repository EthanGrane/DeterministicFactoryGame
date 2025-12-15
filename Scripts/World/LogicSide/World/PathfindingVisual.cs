using System;
using UnityEngine;

public class PathfindingVisual : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private void Start()
    {
        UpdatePathfindingVisual();
    }

    public void UpdatePathfindingVisual()
    {
        Vector2Int[] path = EnemyManager.Instance.GetPath();
        lineRenderer.positionCount = path.Length;

        for (int i = 0; i < path.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3Int(path[i].x, path[i].y, 0));
        }
    }
}
