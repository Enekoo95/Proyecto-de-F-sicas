using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    public float thrust = 20f;
    public float turnSpeed = 2f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical"); 
        rb.AddForce(transform.forward * forwardInput * thrust, ForceMode.Force);

        float turnInput = Input.GetAxis("Horizontal"); 
        rb.AddTorque(Vector3.up * turnInput * turnSpeed, ForceMode.Acceleration);

        float mouseX = Input.GetAxis("Mouse X");
        rb.AddTorque(Vector3.up * mouseX * turnSpeed * 0.5f, ForceMode.Acceleration);
    }
}

