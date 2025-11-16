using UnityEngine;

public class GeneradorObstaculos : MonoBehaviour
{
    public TerrenoProcedural terreno;
    public GameObject prefabObstaculo;
    public int cantidad = 20;

    void Start()
    {
        GenerarObstaculos();
    }

    void GenerarObstaculos()
    {
        Mesh mesh = terreno.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < cantidad; i++)
        {
            // Posición aleatoria dentro del terreno
            int indice = Random.Range(0, vertices.Length);
            Vector3 posTerreno = vertices[indice];

            Vector3 posicionMundo = terreno.transform.TransformPoint(posTerreno);

            Instantiate(prefabObstaculo, posicionMundo, Quaternion.identity);
        }
    }
}
