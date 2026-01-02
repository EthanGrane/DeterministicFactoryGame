using UnityEngine;

public class RectangularRepulsionBounds : MonoBehaviour
{
    public Vector2 minBounds = new Vector2(-20, -20);
    public Vector2 maxBounds = new Vector2(20, 20);

    public float repulsionStrength = 40f;
    public float falloffDistance = 2f;

    PlayerMover player;
    Rigidbody rb;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerMover>();
        rb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 pos = rb.position;
        Vector3 force = Vector3.zero;

        if (pos.x < minBounds.x)
            force += Vector3.right * ComputeForce(minBounds.x - pos.x);

        else if (pos.x > maxBounds.x)
            force += Vector3.left * ComputeForce(pos.x - maxBounds.x);

        if (pos.z < minBounds.y)
            force += Vector3.forward * ComputeForce(minBounds.y - pos.z);

        else if (pos.z > maxBounds.y)
            force += Vector3.back * ComputeForce(pos.z - maxBounds.y);

        if (force.sqrMagnitude > 0f)
            player.AddExternalVelocity(force * Time.fixedDeltaTime);
    }

    float ComputeForce(float distance)
    {
        float t = Mathf.Clamp01(distance / falloffDistance);
        return repulsionStrength * t;
    }
}