using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Polygon2DMeshGenerator : MonoBehaviour
{ 
	public float terrainDepth;
	public Material terrainMaterial;

	private PolygonCollider2D polygonCollider;
	private GameObject generatedObject;

	//public GameObject GenerateMesh()
	//   {
	//	polygonCollider = GetComponent<PolygonCollider2D>();
	//       Vector2[] points = polygonCollider.points;

	//	MeshBuilder meshBuilder = new MeshBuilder();

	//	// Add vertices
	//	for (int i = 0; i < points.Length; i++)
	//	{
	//		// first row of vertices at depth 0
	//		meshBuilder.AddVertex(new Vector3(points[i].x, points[i].y, 0));

	//		// second row of vertices at depth terrainDepth
	//		meshBuilder.AddVertex(new Vector3(points[i].x, points[i].y, terrainDepth));
	//	}

	//	// Add triangles
	//	for (int i = 0; i < points.Length - 1; i++)
	//	{
	//		int index1 = i * 2;
	//		int index2 = i * 2 + 1;
	//		int index3 = i * 2 + 2;
	//		int index4 = i * 2 + 3;

	//		meshBuilder.AddTriangle(index1, index3, index2);
	//		meshBuilder.AddTriangle(index3, index4, index2);
	//	}

	//	// Build the mesh
	//	Mesh mesh = meshBuilder.CreateMesh();

	//	// Create a game object with mesh
	//	GameObject generatedObject = new GameObject();
	//	generatedObject.name = "TrackMesh";
	//	generatedObject.transform.position = transform.position;
	//	generatedObject.transform.rotation = transform.rotation;
	//	generatedObject.transform.localScale = transform.localScale;
	//	generatedObject.AddComponent<MeshRenderer>();
	//	MeshFilter filter = generatedObject.AddComponent<MeshFilter>();
	//	filter.mesh = mesh;
	//	generatedObject.AddComponent<MeshCollider>();

	//	return generatedObject;
	//}

	public GameObject CreateExtrudedMesh()
	{
		polygonCollider = GetComponent<PolygonCollider2D>();

		Vector2[] vertices2D = polygonCollider.GetPath(0); // Get vertices from the polygon collider
		Vector3[] vertices3D = new Vector3[vertices2D.Length * 2]; // Create an array to hold the extruded vertices
		List<int> trianglesList = new List<int>(); // Create a list to hold the triangle indices

		// Create the extruded vertices
		for (int i = 0; i < vertices2D.Length; i++)
		{
			vertices3D[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
			vertices3D[i + vertices2D.Length] = new Vector3(vertices2D[i].x, vertices2D[i].y, terrainDepth);
		}

		// Create the wall triangles for both sides
		for (int i = 0; i < vertices2D.Length; i++)
		{
			int j = (i + 1) % vertices2D.Length;
			int k = i * 6;
			trianglesList.Add(i);
			trianglesList.Add(j);
			trianglesList.Add(j + vertices2D.Length);
			trianglesList.Add(i);
			trianglesList.Add(j + vertices2D.Length);
			trianglesList.Add(i + vertices2D.Length);
			// Duplicate the triangles for the other side of the mesh
			trianglesList.Add(j + vertices2D.Length);
			trianglesList.Add(j);
			trianglesList.Add(i + vertices2D.Length);
			trianglesList.Add(i + vertices2D.Length);
			trianglesList.Add(j + vertices2D.Length);
			trianglesList.Add(j);

			// Create the side triangles for both sides
			int quadOffset = vertices2D.Length * 2;
			int currentVertexIndex = i + quadOffset;
			int nextVertexIndex = j + quadOffset;
			int previousVertexIndex = (i == 0) ? vertices2D.Length * 2 - 1 : currentVertexIndex - 1;
			int oppositeVertexIndex = previousVertexIndex - vertices2D.Length;

			// Create the first triangle of the quad
			trianglesList.Add(currentVertexIndex);
			trianglesList.Add(nextVertexIndex);
			trianglesList.Add(previousVertexIndex);

			// Create the second triangle of the quad
			trianglesList.Add(currentVertexIndex);
			trianglesList.Add(previousVertexIndex);
			trianglesList.Add(oppositeVertexIndex);

			// Duplicate the quad for the other side of the mesh
			currentVertexIndex += vertices2D.Length;
			nextVertexIndex += vertices2D.Length;
			previousVertexIndex += vertices2D.Length;
			oppositeVertexIndex += vertices2D.Length;

			// Create the first triangle of the quad
			trianglesList.Add(currentVertexIndex);
			trianglesList.Add(previousVertexIndex);
			trianglesList.Add(nextVertexIndex);

			// Create the second triangle of the quad
			trianglesList.Add(currentVertexIndex);
			trianglesList.Add(oppositeVertexIndex);
			trianglesList.Add(previousVertexIndex);
		}

		// Create the top and bottom triangles for both sides
		for (int i = 0; i < vertices2D.Length; i++)
		{
			int k = i * 2;
			int vertexIndex1 = i;
			int vertexIndex2 = (i + 1) % vertices2D.Length;
			int vertexIndex3 = vertexIndex1 + vertices2D.Length;
			int vertexIndex4 = vertexIndex2 + vertices2D.Length;
			trianglesList.Add(vertexIndex1);
			trianglesList.Add(vertexIndex2);
			trianglesList.Add(vertexIndex3);

			trianglesList.Add(vertexIndex2);
			trianglesList.Add(vertexIndex4);
			trianglesList.Add(vertexIndex3);

			trianglesList.Add(vertexIndex2);
			trianglesList.Add(vertexIndex1);
			trianglesList.Add(vertexIndex4);

			trianglesList.Add(vertexIndex1);
			trianglesList.Add(vertexIndex3);
			trianglesList.Add(vertexIndex4);
		}

		int[] triangles = trianglesList.ToArray(); // Convert the triangle list to an array

		// Create the mesh and assign the vertices and triangles
		Mesh mesh = new Mesh();
		mesh.vertices = vertices3D;
		mesh.triangles = triangles;

		// Recalculate the normals to make the lighting work correctly
		mesh.RecalculateNormals();

		// Create the game object and add a mesh renderer and filter
		GameObject go = new GameObject("Extruded Mesh");
		MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = go.AddComponent<MeshFilter>();

		// Set the mesh on the filter and assign the material on the renderer
		meshFilter.mesh = mesh;
		meshRenderer.material = terrainMaterial;

		return go;
	}

	public void DestroyGeneratedObject()
	{
		if (generatedObject != null)
		{
			DestroyImmediate(generatedObject);
			generatedObject = null;
		}
	}
}

#if UNITY_EDITOR
public class Polygon2DMeshGeneratorEditor : EditorWindow
{
	[MenuItem("Window/Polygon2DMeshGenerator")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		Polygon2DMeshGeneratorEditor window = (Polygon2DMeshGeneratorEditor)EditorWindow.GetWindow(typeof(Polygon2DMeshGeneratorEditor));
		window.Show();
	}

	private float terrainDepth = 0f;
	private Material terrainMaterial;

	private List<GameObject> generatedObjects = new List<GameObject>();

	void OnGUI()
	{
		terrainDepth = EditorGUILayout.FloatField("Terrain Depth", terrainDepth);
		terrainMaterial = (Material)EditorGUILayout.ObjectField(terrainMaterial, typeof(Material), false);

		if (GUILayout.Button("Generate Meshes"))
		{
			if(generatedObjects.Count > 0)
			{
				// Destroy all generated objects and clear the list
				foreach (GameObject obj in generatedObjects)
				{
					DestroyImmediate(obj);
				}
				generatedObjects.Clear();
			}

			// Find all PolygonCollider2D components in the scene
			PolygonCollider2D[] colliders = FindObjectsOfType<PolygonCollider2D>();

			// Generate a mesh for each PolygonCollider2D
			foreach (PolygonCollider2D collider in colliders)
			{
				Polygon2DMeshGenerator generator;

				if (collider.gameObject.GetComponent<Polygon2DMeshGenerator>() != null)
					generator = collider.gameObject.GetComponent<Polygon2DMeshGenerator>();
				else
					generator = collider.gameObject.AddComponent<Polygon2DMeshGenerator>();

				generator.terrainDepth = terrainDepth;
				generator.terrainMaterial = terrainMaterial;
				GameObject generatedObject = generator.CreateExtrudedMesh();
				generatedObjects.Add(generatedObject);
			}
		}

		if (GUILayout.Button("Delete Meshes"))
		{
			// Destroy all generated objects and clear the list
			foreach (GameObject obj in generatedObjects)
			{
				DestroyImmediate(obj);
			}
			generatedObjects.Clear();
		}
	}
}
#endif