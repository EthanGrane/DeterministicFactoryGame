using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretLogic : BuildingLogic, IItemAcceptor
{
    int projectileRateCount = 0;

    readonly Queue<Item> ammoQueue = new();

    public override void Tick()
    {
        if (ammoQueue.Count == 0) return;

        if (projectileRateCount > 0)
        {
            projectileRateCount--;
            return;
        }

        TurretBlock turretBlock = building.block as TurretBlock;
        Vector3 pos = building.position + Vector2.one * (building.block.size - 1);
        pos.z = pos.y;
        pos.y = 0;

        Enemy[] enemies = EnemyManager.Instance.GetEnemiesOnRadius(pos, turretBlock.turretRange);
        if (enemies.Length == 0) return;
        
        Enemy nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (var e in enemies)
        {
            float d = Vector3.Distance(pos, e.GetPosition());
            if (d < nearestDistance)
            {
                nearestDistance = d;
                nearestEnemy = e;
            }
        }

        if (nearestEnemy == null) return;

        // 🔫 Consumir munición
        Item ammoItem = ammoQueue.Dequeue();
        ProjectileSO projectile = ammoItem.projectile;

        if (projectile == null)
            return;

        Vector3 enemyPos = nearestEnemy.GetPosition();
        Vector3 enemyVel = nearestEnemy.GetVelocity();

        float travelTime = nearestDistance / projectile.speed;
        Vector3 predictedPos = enemyPos + enemyVel * travelTime;
        Vector3 direction = (predictedPos - pos).normalized;

        projectileRateCount = turretBlock.projectileRateOnTicks;

        ProjectileManager.Instance.SpawnProjectile(
            pos,
            direction,
            projectile
        );
    }

    // ================= IItemAcceptor =================

    public bool CanAccept(Item item)
    {
        TurretBlock turretBlock = building.block as TurretBlock;

        if (ammoQueue.Count >= turretBlock.maxAmmo)
            return false;

        if (!item.isAmmo || item.projectile == null)
            return false;

        return turretBlock.avaliableProjectiles.Contains(item.projectile);
    }
    public bool Insert(Item item)
    {
        if (!CanAccept(item))
            return false;

        ammoQueue.Enqueue(item);
        return true;
    }
}
