using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 150f;
    public Transform cameraTransform;
    public float gravity = 9.81f;
    public float jumpForce = 8f;

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private PlataformaMovil plataformaActual;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovimientoJugador();
        RotacionCamara();
        AplicarGravedadYSalto();
        MoverConPlataforma();
    }

    //-----------------------------
    // MOVIMIENTO + SALTO
    //-----------------------------
    void MovimientoJugador()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 mov = transform.right * moveX + transform.forward * moveZ;
        controller.Move(mov * speed * Time.deltaTime);

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
        }
    }

    //-----------------------------
    // CÁMARA
    //-----------------------------
    void RotacionCamara()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    void AplicarGravedadYSalto()
    {
        bool grounded = EsSuelo();

        // SALTO
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
        }

        // GRAVEDAD
        if (grounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    bool EsSuelo()
    {
        // Lanza un rayo desde los pies para detectar suelo
        return Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            0.3f
        );
    }




    //-----------------------------
    // EMPUJAR OBJETOS Y PLATAFORMAS
    //-----------------------------
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Empujar rigidbodies
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null && !rb.isKinematic)
        {
            Vector3 fuerza = hit.moveDirection * 5f;
            rb.AddForce(fuerza, ForceMode.Impulse);
        }

        // Detectar plataformas móviles
        PlataformaMovil p = hit.collider.GetComponent<PlataformaMovil>();
        if (p != null)
            plataformaActual = p;
    }

    //-----------------------------
    // MOVERSE CON LA PLATAFORMA
    //-----------------------------
    void MoverConPlataforma()
    {
        if (plataformaActual != null)
            controller.Move(plataformaActual.DeltaMovimiento);

        if (!controller.isGrounded)
            plataformaActual = null;
    }
}
