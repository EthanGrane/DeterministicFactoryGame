using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float acceleration = 30f;
    public float deceleration = 0.15f;
    public float externalDamping = 8f;

    Rigidbody rb;

    Vector3 moveInputs;
    Vector3 velocity;
    Vector3 smoothDampVelocity;

    Vector3 externalVelocity; // 👈 NUEVO
    Vector3 lookDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation |
                         RigidbodyConstraints.FreezePositionY;
    }

    public void AddExternalVelocity(Vector3 v)
    {
        externalVelocity += v;
    }

    void Update()
    {
        moveInputs = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (moveInputs.sqrMagnitude > 0.01f)
            lookDirection = Vector3.Lerp(
                lookDirection,
                moveInputs,
                Time.deltaTime * 15f
            );
    }

    void FixedUpdate()
    {
        if (moveInputs.sqrMagnitude > 0.01f)
        {
            velocity += moveInputs * (acceleration * Time.fixedDeltaTime);
            velocity = Vector3.ClampMagnitude(velocity, moveSpeed);
        }
        else
        {
            velocity = Vector3.SmoothDamp(
                velocity,
                Vector3.zero,
                ref smoothDampVelocity,
                deceleration
            );
        }

        // 🔹 amortiguar fuerzas externas
        externalVelocity = Vector3.Lerp(
            externalVelocity,
            Vector3.zero,
            Time.fixedDeltaTime * externalDamping
        );

        rb.linearVelocity = velocity + externalVelocity;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDirection, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,
                targetRot,
                Time.fixedDeltaTime * 10f
            ));
        }
    }
}
