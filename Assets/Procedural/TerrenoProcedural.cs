using UnityEngine;

/// <summary>
/// Genera un terreno procedural mediante Perlin Noise como malla en tiempo de ejecución.
/// - Attach a un GameObject vacío.
/// - Añadir un material (terrainMaterial) para ver el terreno.
/// - Pulsar Play o llamar a Generate() desde otro script.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrenoProcedural : MonoBehaviour
{
    [Header("Tamaño de la malla")]
    public int ancho = 100;           // número de vértices en X
    public int largo = 100;           // número de vértices en Z
    public float escala = 5f;         // escala del noise (mayor = más suave)
    public float alturaMax = 8f;      // altura máxima del terreno

    [Header("Perlin Noise")]
    public float offsetX = 0f;
    public float offsetZ = 0f;
    public int seed = 0;

    [Header("Material")]
    public Material terrenoMaterial;

    // Internos
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private MeshFilter mf;
    private MeshCollider mc;

    void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
        if (terrenoMaterial != null)
            GetComponent<MeshRenderer>().material = terrenoMaterial;
        Generate();
    }

    [ContextMenu("Generate Terrain")]
    public void Generate()
    {
        Random.InitState(seed);
        mesh = new Mesh();
        mesh.name = "TerrenoProcedural_Mesh";

        vertices = new Vector3[(ancho + 1) * (largo + 1)];
        uvs = new Vector2[vertices.Length];

        // Generar vértices
        int i = 0;
        for (int z = 0; z <= largo; z++)
        {
            for (int x = 0; x <= ancho; x++)
            {
                float sampleX = (x + offsetX + seed * 100f) / escala;
                float sampleZ = (z + offsetZ + seed * 100f) / escala;
                float noise = Mathf.PerlinNoise(sampleX, sampleZ);
                float y = noise * alturaMax;
                vertices[i] = new Vector3(x - ancho * 0.5f, y, z - largo * 0.5f); // centrar en (0,0,0)
                uvs[i] = new Vector2((float)x / ancho, (float)z / largo);
                i++;
            }
        }

        // Triangles
        triangles = new int[ancho * largo * 6];
        int vert = 0;
        int tri = 0;
        for (int z = 0; z < largo; z++)
        {
            for (int x = 0; x < ancho; x++)
            {
                triangles[tri + 0] = vert + 0;
                triangles[tri + 1] = vert + ancho + 1;
                triangles[tri + 2] = vert + 1;

                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + ancho + 1;
                triangles[tri + 5] = vert + ancho + 2;

                vert++;
                tri += 6;
            }
            vert++;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mf.sharedMesh = mesh;
        mc.sharedMesh = mesh; // collider coincide con la malla
    }

    // Devuelve altura del terreno en una posición XZ local (interpolada)
    public float GetAlturaEnPosicion(Vector3 worldPos)
    {
        // Convertir world a local
        Vector3 local = transform.InverseTransformPoint(worldPos);
        // Transformar a coordenadas de índice
        float px = local.x + ancho * 0.5f;
        float pz = local.z + largo * 0.5f;
        if (px < 0 || pz < 0 || px > ancho || pz > largo) return transform.position.y;

        // Interpolación bilineal entre vértices cercanos
        int x0 = Mathf.FloorToInt(px);
        int z0 = Mathf.FloorToInt(pz);
        int x1 = Mathf.Clamp(x0 + 1, 0, ancho);
        int z1 = Mathf.Clamp(z0 + 1, 0, largo);

        int idx00 = z0 * (ancho + 1) + x0;
        int idx10 = z0 * (ancho + 1) + x1;
        int idx01 = z1 * (ancho + 1) + x0;
        int idx11 = z1 * (ancho + 1) + x1;

        float tx = px - x0;
        float tz = pz - z0;

        float h00 = vertices[idx00].y;
        float h10 = vertices[idx10].y;
        float h01 = vertices[idx01].y;
        float h11 = vertices[idx11].y;

        float h0 = Mathf.Lerp(h00, h10, tx);
        float h1 = Mathf.Lerp(h01, h11, tx);
        float h = Mathf.Lerp(h0, h1, tz);
        return transform.position.y + h;
    }
}
