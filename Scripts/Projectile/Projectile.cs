using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    public Vector3 position;
    public Vector3 direction;

    public float speed;
    public float lifetime;
    public float collisionRadius;

    public int damage;
    public int penetration;

    public bool isDead;

    public HashSet<Enemy> hitEnemies = new();

    public Projectile(Vector3 pos, Vector3 dir, ProjectileSO data)
    {
        // Se asegura que todo sea plano XZ
        position = new Vector3(pos.x, 0f, pos.z);
        direction = new Vector3(dir.x, 0f, dir.z).normalized;

        speed = data.speed;
        lifetime = data.lifetime;
        collisionRadius = data.collisionRadius;

        damage = data.damage;
        penetration = data.penetration;
        isDead = false;
    }
}