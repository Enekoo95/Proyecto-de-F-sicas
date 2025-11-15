using UnityEngine;

public class CamaraControladora : MonoBehaviour
{
    public ActionHub actionHub;
    public Transform jugador;          // objeto raíz del jugador
    public Transform camaraPivot;      // pivot para rotación vertical
    public float sensibilidadX = 2f;
    public float sensibilidadY = 2f;
    public float limitePitch = 80f;

    private float rotX = 0f;
    private float rotY = 0f;

    void Start()
    {
        if (actionHub == null)
            actionHub = FindObjectOfType<ActionHub>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Seguir al jugador
        transform.position = jugador.position;

        Vector2 mirar = actionHub.Mirar;

        // Rotación horizontal (yaw)  solo el jugador
        rotY += mirar.x * sensibilidadX;
        jugador.rotation = Quaternion.Euler(0f, rotY, 0f);

        // Rotación vertical (pitch)  solo el pivot
        rotX -= mirar.y * sensibilidadY;
        rotX = Mathf.Clamp(rotX, -limitePitch, limitePitch);
        camaraPivot.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    }
}
