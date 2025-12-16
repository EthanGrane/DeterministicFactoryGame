using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    public Vector2 position;
    public Vector2 direction;

    public float speed;
    public float lifetme;
    public float collisionRadius;

    public int damage;
    public int penetration;

    public bool isDead;

    public HashSet<Enemy> hitEnemies = new();

    public Projectile(Vector2 pos, Vector2 dir, ProjectileSO data)
    {
        position = pos;
        direction = dir.normalized;

        speed = data.speed;
        lifetme = data.lifetime;
        collisionRadius = data.collisionRadius;

        damage = data.damage;
        penetration = data.penetration;
        isDead = false;
    }
}