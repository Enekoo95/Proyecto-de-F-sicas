using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 8f;
    public float gravity = 9.81f;

    [Header("Cámara")]
    public Transform pivotCamara;
    public float mouseSensitivity = 150f;

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
        AplicarFisicas();
        SeguirPlataforma();
        VerificarPlataformaDebajo();
    }

    // -----------------------------
    // MOVIMIENTO
    // -----------------------------
    void MovimientoJugador()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 mov = transform.right * moveX + transform.forward * moveZ;
        controller.Move(mov * speed * Time.deltaTime);
    }

    // -----------------------------
    // ROTACIÓN DE CÁMARA
    // -----------------------------
    void RotacionCamara()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -75f, 75f);

        pivotCamara.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    // -----------------------------
    // SALTO + GRAVEDAD
    // -----------------------------
    void AplicarFisicas()
    {
        bool grounded = EstaEnSuelo();

        if (grounded && velocity.y < 0)
            velocity.y = -2f; // Mantener contacto con el suelo

        if (grounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y = jumpForce;

        // Gravedad
        velocity.y -= gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // -----------------------------
    // DETECTAR PLATAFORMAS
    // -----------------------------
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        PlataformaMovil p = hit.collider.GetComponent<PlataformaMovil>();
        if (p != null)
            plataformaActual = p;
    }

    // -----------------------------
    // SEGUIR PLATAFORMA
    // -----------------------------
    void SeguirPlataforma()
    {
        if (plataformaActual != null)
            controller.Move(plataformaActual.DeltaMovimiento);
    }

    // -----------------------------
    // VERIFICAR SI EL JUGADOR SIGUE SOBRE LA PLATAFORMA
    // -----------------------------
    void VerificarPlataformaDebajo()
    {
        if (plataformaActual == null) return;

        // Raycast desde los pies hacia abajo
        if (!Physics.Raycast(transform.position, Vector3.down, controller.height / 2 + 0.1f))
        {
            plataformaActual = null; // ya no hay plataforma debajo
        }
    }

    // -----------------------------
    // FUNCION DE GROUND CHECK
    // -----------------------------
    bool EstaEnSuelo()
    {
        // Raycast desde los pies para detectar cualquier plataforma o suelo
        return Physics.Raycast(transform.position, Vector3.down, controller.height / 2 + 0.1f);
    }
}
