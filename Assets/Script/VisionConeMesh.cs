using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class VisionConeMesh : MonoBehaviour
{
    public EnemyVisionChargeOnly enemy; // “G‚ÌŽ‹ŠEƒXƒNƒŠƒvƒg
    public int segments = 40;
    public float yOffset = 0.02f;

    Mesh mesh;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void LateUpdate()
    {
        if (enemy == null) return;

        transform.localPosition = new Vector3(0f, yOffset, 0f);
        transform.localRotation = Quaternion.identity;

        BuildMesh(enemy.viewDistance, enemy.viewHalfAngle);
    }

    void BuildMesh(float radius, float halfAngle)
    {
        int vertCount = segments + 2;
        Vector3[] verts = new Vector3[vertCount];
        int[] tris = new int[segments * 3];

        verts[0] = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float ang = Mathf.Lerp(-halfAngle, halfAngle, t) * Mathf.Deg2Rad;

            float x = Mathf.Sin(ang) * radius;
            float z = Mathf.Cos(ang) * radius;
            verts[i + 1] = new Vector3(x, 0f, z);
        }

        int tri = 0;
        for (int i = 0; i < segments; i++)
        {
            tris[tri++] = 0;
            tris[tri++] = i + 1;
            tris[tri++] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}
