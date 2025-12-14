using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public EnemyTierSO currentTierSo;
    public float collisionRadius = 0.5f;
    [HideInInspector] public float moveSpeed = 2f;

    private bool isAlive = true;
    private Vector2 lastDir;

    private void Start()
    {
        EnemyManager.Instance.ApplyTier(this, currentTierSo);
    }

    private void Update()
    {
        if (!isAlive) return;

        Vector2 dir = EnemyManager.Instance.GetFlowDirection(transform.position);
        if (dir == Vector2.zero) return;

        dir = Vector2.Lerp(lastDir, dir, 0.2f);
        lastDir = dir;

        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
    }

    public void TakeDamage(int dmg)
    {
        EnemyManager.Instance.ProcessDamage(this, dmg);
    }

    public void DieExtern()
    {
        if (!isAlive) return;

        isAlive = false;
        Destroy(gameObject);
    }
    
    public Vector2 GetPosition() => transform.position;
    public Vector2 GetVelocity() => lastDir * moveSpeed;
}