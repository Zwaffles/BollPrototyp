using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteShapeToMesh : MonoBehaviour
{
    private SpriteShape _spriteShape;
    private MeshFilter _meshFilter;
    private PolygonCollider2D polygonCollider;

    private void Start()
    {
        // get the sprite shape, mesh filter and polygon collider components
        _spriteShape = GetComponent<SpriteShape>();
        _meshFilter = GetComponent<MeshFilter>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        // create a new mesh
        Mesh mesh = new Mesh();
        _meshFilter.mesh = mesh;

        // get the vertex data from the polygon collider
        Vector2[] points = polygonCollider.points;

        // convert the points to 3D vector and set the mesh's vertices
        Vector3[] vertices = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            vertices[i] = new Vector3(points[i].x, points[i].y, 0);
        }
        mesh.vertices = vertices;

        // set the mesh's UVs and triangles
        mesh.uv = new Vector2[vertices.Length];
        mesh.triangles = Triangulate(vertices);
        mesh.RecalculateNormals();
    }

    private int[] Triangulate(Vector3[] vertices)
    {
        List<int> triangles = new List<int>();

        // create a list of vertices as Vector2
        Vector2[] polygon = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            polygon[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        // triangulate the polygon using Unity's built-in triangulation method
        Triangulator tr = new Triangulator(polygon);
        int[] result = tr.Triangulate();
        triangles.AddRange(result);

        return triangles.ToArray();
    }
}
