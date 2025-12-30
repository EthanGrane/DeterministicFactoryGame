using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public ProjectileSO projectile;
    public float timeBetweenShots = 0.5f;

    float time;

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {

            time = timeBetweenShots;

            if (Camera.main == null)
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (!groundPlane.Raycast(ray, out float enter))
            {
                return;
            }

            Vector3 mouseWorldPos = ray.GetPoint(enter);

            Vector3 dir = mouseWorldPos - transform.position;
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