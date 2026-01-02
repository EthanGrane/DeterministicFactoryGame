using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PathfindingVisual : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public Color planningColor;
    public Color spawningColor;
    
    private void Start()
    {
        EnemyManager.Instance.onPathChanged += UpdatePathfindingVisual;
        EnemyWavesManager.Instance.onPhaseChanged += ModifyLineColor;
    }

    public void UpdatePathfindingVisual()
    {
        Vector2Int[] path = EnemyManager.Instance.GetPath();
        lineRenderer.positionCount = path.Length;

        for (int i = 0; i < path.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(path[i].x + .5f, .1f, path[i].y + .5f));
        }
    }

    void ModifyLineColor(WavePhase wavePhase)
    {
        switch (wavePhase)
        {
            case WavePhase.Spawning:
                lineRenderer.startColor = spawningColor;
                lineRenderer.endColor = spawningColor;
                lineRenderer.endWidth = .75f;
                lineRenderer.startWidth = .75f;
                break;
            
            case WavePhase.WaitingNext:
                lineRenderer.startColor = spawningColor;
                lineRenderer.endColor = spawningColor;
                lineRenderer.endWidth = .5f;
                lineRenderer.startWidth = .5f;
                break;
            
            case WavePhase.Planning:
                lineRenderer.startColor = planningColor;
                lineRenderer.endColor = planningColor;
                lineRenderer.endWidth = .25f;
                lineRenderer.startWidth = .25f;
                break;
            
            default:
                lineRenderer.startColor = planningColor;
                lineRenderer.endColor = planningColor;
            break;
        }
    }
}
