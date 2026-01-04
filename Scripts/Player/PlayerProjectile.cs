using System;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public ProjectileSO projectile;
    public float timeBetweenShots = 0.5f;
    public float radius;
    public EnemySortType type;
    
    private float time;


    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            return;
        }

        if(EnemyManager.Instance.GetEnemiesAliveCount() == 0)
            return;
        else
        {

            time = timeBetweenShots;

            Enemy targetEnemy = EnemyManager.Instance.GetEnemyOnRadius(transform.position, radius, type);

            if(targetEnemy == null)
                return;
            
            Vector3 dir = targetEnemy.GetPosition() - transform.position;
            dir.y = 0;

            if (dir.sqrMagnitude < 0.0001f)
            {
                return;
            }

            dir.Normalize();

            if (ProjectileManager.Instance == null)
            {
                return;
            }
            
            ProjectileManager.Instance.SpawnProjectile(
                transform.position,
                dir,
                projectile
            );
        }
    }
}