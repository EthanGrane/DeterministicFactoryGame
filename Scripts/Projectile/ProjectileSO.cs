using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "FACTORY/COMBAT/Projectile")]
public class ProjectileSO : ScriptableObject
{
    [Header("Damage")]
    public int damage = 1;
    public int penetration = 1;

    [Header("Movement")]
    public float speed = 10f;
    public float lifetime = 1f;
    public float collisionRadius = 0.25f;
    
    [Header("Visuals")]
    public Color projectileColor = Color.yellow;

}