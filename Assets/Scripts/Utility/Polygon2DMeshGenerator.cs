using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Polygon2DMeshGenerator : MonoBehaviour
{ 
	public float terrainDepth;

	private PolygonCollider2D polygonCollider;
	private GameObject generatedObject;

	public GameObject GenerateMesh()
    {
		polygonCollider = GetComponent<PolygonCollider2D>();
        Vector2[] points = polygonCollider.points;

		MeshBuilder meshBuilder = new MeshBuilder();

		// Add vertices
		for (int i = 0; i < points.Length; i++)
		{
			// first row of vertices at depth 0
			meshBuilder.AddVertex(new Vector3(points[i].x, points[i].y, 0));

			// second row of vertices at depth terrainDepth
			meshBuilder.AddVertex(new Vector3(points[i].x, points[i].y, terrainDepth));
		}

		// Add triangles
		for (int i = 0; i < points.Length - 1; i++)
		{
			int index1 = i * 2;
			int index2 = i * 2 + 1;
			int index3 = i * 2 + 2;
			int index4 = i * 2 + 3;

			meshBuilder.AddTriangle(index1, index3, index2);
			meshBuilder.AddTriangle(index3, index4, index2);
		}

		// Build the mesh
		Mesh mesh = meshBuilder.CreateMesh();

		// Create a game object with mesh
		GameObject generatedObject = new GameObject();
		generatedObject.name = "TrackMesh";
		generatedObject.transform.position = transform.position;
		generatedObject.transform.rotation = transform.rotation;
		generatedObject.transform.localScale = transform.localScale;
		generatedObject.AddComponent<MeshRenderer>();
		MeshFilter filter = generatedObject.AddComponent<MeshFilter>();
		filter.mesh = mesh;
		generatedObject.AddComponent<MeshCollider>();

		return generatedObject;
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
	private List<GameObject> generatedObjects = new List<GameObject>();

	void OnGUI()
	{
		terrainDepth = EditorGUILayout.FloatField("Terrain Depth", terrainDepth);

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
				GameObject generatedObject = generator.GenerateMesh();
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