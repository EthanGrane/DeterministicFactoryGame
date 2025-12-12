using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWavesManager : MonoBehaviour
{
    public EnemyWaveSO[] waves;
    public GameObject enemyPrefab;
    public bool autoWave = true;

    public Transform enemySpawnPosition;
    int currentWave = 0;

    private void Start()
    {
        EnemyManager.Instance.onAllEnemiesDead += () =>
        {
            currentWave++;
            StartWaveRound();
        };
    }

    [ContextMenu("StartWave")]
    public void StartWaveRound()
    {
        for (int i = 0; i < waves[currentWave].waves.Length; i++)
        {
            EnemyWaveSquad waveSquad = waves[currentWave].waves[i];
            StartCoroutine(SpawnWave(waveSquad));
        }
    }

    IEnumerator SpawnWave(EnemyWaveSquad waveSquad)
    {
        for (int j = 0; j < waveSquad.quantity; j++)
        {
            Vector3 offsetPosition = Random.insideUnitSphere * 5;
            Enemy enemy = Instantiate(enemyPrefab,enemySpawnPosition.position + offsetPosition, Quaternion.identity).GetComponent<Enemy>();
            enemy.enemyWave = currentWave;
            EnemyManager.Instance.ApplyTier(enemy, waveSquad.enemy);
            
            yield return new WaitForSeconds(1f/waveSquad.quantity);
        }
    }
}
