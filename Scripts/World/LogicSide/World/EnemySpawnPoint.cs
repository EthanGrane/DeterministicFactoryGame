using System;
using UnityEditor;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    private void Start()
    {
        Pathfinding.Instance.SetStartPoint(new Vector2Int((int)transform.position.x, (int)transform.position.y));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
