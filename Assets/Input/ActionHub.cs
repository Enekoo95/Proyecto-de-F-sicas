using UnityEngine;
using UnityEngine.InputSystem;

public class ActionHub : MonoBehaviour
{
    public static ActionHub Instancia;

    // Wrapper generado automáticamente
    private AccionesJugador acciones;

    // Valores actuales que usarán los controladores
    public Vector2 Movimiento { get; private set; }
    public Vector2 Mirar { get; private set; }
    public bool Saltar { get; private set; }
    public bool Correr { get; private set; }
    public bool EstaPausado { get; private set; }

    private void Awake()
    {
        if (Instancia == null) Instancia = this;
        acciones = new AccionesJugador();
    }

    private void OnEnable()
    {
        acciones.Enable();

        acciones.Jugador.Move.performed += ctx => Movimiento = ctx.ReadValue<Vector2>();
        acciones.Jugador.Move.canceled += ctx => Movimiento = Vector2.zero;

        acciones.Jugador.Look.performed += ctx => Mirar = ctx.ReadValue<Vector2>();
        acciones.Jugador.Look.canceled += ctx => Mirar = Vector2.zero;

        acciones.Jugador.Jump.performed += ctx => Saltar = true;
        acciones.Jugador.Jump.canceled += ctx => Saltar = false;

        acciones.Jugador.Run.performed += ctx => Correr = true;
        acciones.Jugador.Run.canceled += ctx => Correr = false;

        acciones.Jugador.Pause.performed += ctx => TogglePausa();
    }

    private void OnDisable()
    {
        acciones.Disable();
    }

    private void TogglePausa()
    {
        EstaPausado = !EstaPausado;

        if (EstaPausado) DesactivarInputs();
        else ActivarInputs();
    }

    public void DesactivarInputs()
    {
        acciones.Jugador.Disable();
    }

    public void ActivarInputs()
    {
        acciones.Jugador.Enable();
    }
}
