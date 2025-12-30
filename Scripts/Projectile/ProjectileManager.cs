using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;
    public List<Projectile> projectiles = new();

    ProjectileVisual projectileVisual;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        projectileVisual = GetComponent<ProjectileVisual>();
    }

    public void SpawnProjectile(Vector3 pos, Vector3 dir, ProjectileSO data)
    {
        if (data == null) return;

        pos.y = 0;
        dir.y = 0;

        Projectile p = new Projectile(pos, dir.normalized, data);

        if (projectileVisual != null)
            projectileVisual.RegisterProjectile(p, data);

        projectiles.Add(p);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // Actualizar posiciones y lifetimes
        for (int i = 0; i < projectiles.Count; i++)
        {
            Projectile p = projectiles[i];

            p.position += p.direction * p.speed * dt;
            p.lifetime -= dt;

            if (p.lifetime <= 0f || p.isDead)
            {
                if (projectileVisual != null)
                    projectileVisual.UnregisterProjectile(p);

                projectiles.RemoveAt(i);
                i--;
            }
        }

        CheckCollisionDetection();
    }

    private void CheckCollisionDetection()
    {
        Enemy[] enemies = EnemyManager.Instance.enemies.ToArray();
        if (enemies.Length == 0) return;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i];
            Vector3 enemyPos = enemy.transform.position;
            enemyPos.y = 0; // Ignorar altura

            for (int j = 0; j < projectiles.Count; j++)
            {
                Projectile p = projectiles[j];
                Vector3 projPos = p.position;
                projPos.y = 0;

                float dist = Vector3.Distance(projPos, enemyPos);
                if (dist <= enemy.collisionRadius + p.collisionRadius)
                {
                    if (p.hitEnemies.Contains(enemy)) continue;

                    p.hitEnemies.Add(enemy);
                    EnemyManager.Instance.ProcessDamage(enemy, p);

                    p.penetration--;
                    if (p.penetration <= 0)
                        p.isDead = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (projectiles == null) return;

        Gizmos.color = Color.red;
        foreach (var p in projectiles)
        {
            Vector3 pos = p.position;
            pos.y = 0;
            Gizmos.DrawWireSphere(pos, p.collisionRadius);
        }
    }
}
