using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed;
    public float remainingRange;
    public float collisionRadius = .33f;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - (Vector2)transform.position;
            direction = direction.normalized;
            Projectile projectile = new Projectile(transform.position, direction, speed, collisionRadius, remainingRange);
            ProjectileManager.instance.RegisterProjectile(projectile);
        }
    }
}
