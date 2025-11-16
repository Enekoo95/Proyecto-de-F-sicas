using UnityEngine;

public class EmpujarObjeto : MonoBehaviour
{
    public Transform camara;
    public float distanciaEmpuje = 2f;
    public float fuerzaEmpuje = 10f;   // Fuerza simulada: FUERZA DE EMPUJE (impulso aplicado al objeto)
    public KeyCode botonEmpujar = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(botonEmpujar))
        {
            IntentarEmpujar();
        }
    }

    void IntentarEmpujar()
    {
        Ray ray = new Ray(camara.position, camara.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaEmpuje))
        {
            Rigidbody rb = hit.rigidbody;

            if (rb != null && !rb.isKinematic)
            {
                Vector3 direccion = camara.forward;
                direccion.y = 0; // Evitar empujar hacia arriba

                // Se aplica una fuerza de empuje al objeto detectado
                rb.AddForce(direccion.normalized * fuerzaEmpuje, ForceMode.Impulse);

                Debug.Log("Empujando " + hit.collider.name);
            }
        }
    }
}
