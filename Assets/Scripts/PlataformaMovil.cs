using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Vector3 puntoA;
    public Vector3 puntoB;
    public float velocidad = 2f;

    private Vector3 posAnterior;

    public Vector3 DeltaMovimiento => transform.position - posAnterior;

    void Start()
    {
        posAnterior = transform.position;
    }

    void Update()
    {
        posAnterior = transform.position;

        float t = (Mathf.Sin(Time.time * velocidad) + 1f) / 2f;
        transform.position = Vector3.Lerp(puntoA, puntoB, t);
    }
}
