using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipGravityAlign : MonoBehaviour
{
    [Header("Gravedad")]
    public Transform planet; 
    public float gravityStrength = 9.81f;

    [Header("Alineacion tipo muelle")]
    public float alignStrength = 10f; 
    public float alignDamping = 2f;   

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {

        Vector3 gravityDir = planet? (planet.position - transform.position).normalized: Vector3.down;

        rb.AddForce(gravityDir * gravityStrength * rb.mass, ForceMode.Force);

        Vector3 desiredUp = -gravityDir;

        Quaternion currentRot = transform.rotation;
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, desiredUp) * currentRot;

        Quaternion deltaRot = targetRot * Quaternion.Inverse(currentRot);
        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 180f) angle -= 360f;

        if (Mathf.Abs(angle) > 0.01f)
        {
            Vector3 angularAcceleration = axis.normalized * angle * Mathf.Deg2Rad * alignStrength - rb.angularVelocity * alignDamping;
            rb.AddTorque(angularAcceleration, ForceMode.Acceleration);
        }

        Debug.DrawRay(transform.position, -gravityDir * 3f, Color.yellow); 
        Debug.DrawRay(transform.position, desiredUp * 3f, Color.green);   
        Debug.DrawRay(transform.position, transform.up * 3f, Color.cyan);

        float targetHeight = 5f;
        float springStrength = 300f;
        float springDamping = 5f;

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, targetHeight * 2f))
        {
            float heightError = targetHeight - hit.distance;
            float velocityAlongNormal = Vector3.Dot(rb.linearVelocity, transform.up);
            float lift = (heightError * springStrength) - (velocityAlongNormal * springDamping);
            rb.AddForce(transform.up * lift, ForceMode.Force);
        }
    }
}
