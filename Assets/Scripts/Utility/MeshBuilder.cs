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

        // Create a convex hull from the vertices using the Graham scan algorithm
        List<Vector3> hullVertices = new List<Vector3>();
        List<Vector2> points = new List<Vector2>();
        foreach (Vector3 vertex in vertices)
        {
            points.Add(new Vector2(vertex.x, vertex.y));
        }

        List<Vector2> hullPoints = GrahamScan(points);
        foreach (Vector2 hullPoint in hullPoints)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (hullPoint.x == vertices[i].x && hullPoint.y == vertices[i].y)
                {
                    hullVertices.Add(vertices[i]);
                    break;
                }
            }
        }

        // Rebuild triangles based on the updated hull vertices
        List<int> updatedTriangles = new List<int>();
        for (int i = 0; i < triangles.Count; i += 3)
        {
            int vertex1 = triangles[i];
            int vertex2 = triangles[i + 1];
            int vertex3 = triangles[i + 2];

            if (!hullVertices.Contains(vertices[vertex1]) ||
                !hullVertices.Contains(vertices[vertex2]) ||
                !hullVertices.Contains(vertices[vertex3]))
            {
                // Skip this triangle, as it contains at least one vertex that is not in the updated hull
                continue;
            }

            int index1 = hullVertices.IndexOf(vertices[vertex1]);
            int index2 = hullVertices.IndexOf(vertices[vertex2]);
            int index3 = hullVertices.IndexOf(vertices[vertex3]);

            if (IsLeftTurn(hullVertices[index1], hullVertices[index2], hullVertices[index3]))
            {
                // Add triangle with clockwise winding order
                updatedTriangles.Add(index1);
                updatedTriangles.Add(index3);
                updatedTriangles.Add(index2);
            }
            else
            {
                // Add triangle with counterclockwise winding order
                updatedTriangles.Add(index1);
                updatedTriangles.Add(index2);
                updatedTriangles.Add(index3);
            }
        }

        // Assign vertices and triangles to the mesh
        mesh.vertices = hullVertices.ToArray();
        mesh.triangles = updatedTriangles.ToArray();

        // Recalculate normals and bounds
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    // Graham scan algorithm to find the convex hull of a set of 2D points
    List<Vector2> GrahamScan(List<Vector2> points)
    {
        if (points.Count < 3)
        {
            return points;
        }

        List<Vector2> hull = new List<Vector2>();

        // Find the point with the lowest y-coordinate
        Vector2 lowestPoint = points[0];
        for (int i = 1; i < points.Count; i++)
        {
            if (points[i].y < lowestPoint.y || (points[i].y == lowestPoint.y && points[i].x < lowestPoint.x))
            {
                lowestPoint = points[i];
            }
        }

        hull.Add(lowestPoint);

        // Sort the remaining points in order of their polar angles with respect to the lowest point
        points.Sort((a, b) =>
        {
            float angleA = Mathf.Atan2(a.y - lowestPoint.y, a.x - lowestPoint.x);
            float angleB = Mathf.Atan2(b.y - lowestPoint.y, b.x - lowestPoint.x);
            if (angleA < angleB)
            {
                return -1;
            }
            else if (angleA > angleB)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        });

        hull.Add(points[1]);

        for (int i = 2; i < points.Count; i++)
        {
            while (hull.Count >= 2 && !IsLeftTurn(hull[hull.Count - 2], hull[hull.Count - 1], points[i]))
            {
                hull.RemoveAt(hull.Count - 1);
            }
            hull.Add(points[i]);
        }

        return hull;
    }

    // Check whether three points make a left turn
    bool IsLeftTurn(Vector2 a, Vector2 b, Vector2 c)
    {
        float crossProduct = (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
        return crossProduct > 0;
    }
}
