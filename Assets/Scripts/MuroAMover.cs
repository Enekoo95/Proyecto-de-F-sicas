using UnityEngine;

public class MuroAMover : MonoBehaviour
{
    public Rigidbody rb;          // Rigidbody de la pared
    public float amplitud = 3f;   // Cuánto sube y baja
    public float velocidad = 2f;  // Velocidad de oscilación

    private Vector3 posInicial;

    void Start()
    {
        posInicial = rb.position; // Guardar posición inicial
    }

    void FixedUpdate()
    {
        // Movimiento vertical tipo seno
        Vector3 nuevaPos = posInicial + Vector3.up * Mathf.Sin(Time.time * velocidad) * amplitud;
        rb.MovePosition(nuevaPos);
    }
}
