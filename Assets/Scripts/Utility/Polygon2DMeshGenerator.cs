using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon2DMeshGenerator : MonoBehaviour
{
    [SerializeField]
    private PolygonCollider2D polygonCollider;

	[SerializeField]
	private float terrainDepth;

    void Start()
    {
        // get the vertex data from the polygon collider
        Vector2[] points = polygonCollider.points;

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(points);
		int[] indices = tr.Triangulate();

		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[points.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = new Vector3(points[i].x, points[i].y, terrainDepth);
		}

		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();

		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;
		gameObject.AddComponent(typeof(MeshCollider));
	}
}