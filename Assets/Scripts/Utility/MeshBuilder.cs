using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
    }

    public void AddTriangle(int index1, int index2, int index3)
    {
        triangles.Add(index1);
        triangles.Add(index2);
        triangles.Add(index3);
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        // Create a convex hull from the vertices
        List<Vector3> hullVertices = new List<Vector3>();
        int[] hullIndices = QuickHull.GetConvexHullIndices(vertices.ToArray());
        for (int i = 0; i < hullIndices.Length; i++)
        {
            hullVertices.Add(vertices[hullIndices[i]]);
        }

        // Assign vertices and triangles to the mesh
        mesh.vertices = hullVertices.ToArray();
        mesh.triangles = triangles.ToArray();

        // Recalculate normals and bounds
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
