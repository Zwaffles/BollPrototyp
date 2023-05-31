using UnityEngine;
using PathCreation;

/// <summary>
/// Generates a mesh for a road track based on a PathCreator component.
/// </summary>
public class TrackMeshCreator : PathSceneTool
{
    [Header("Road settings"), Min(0f)]
    public float roadHeight = 1.5f;
    [Min(0f)]
    public float roadWidth = 4f;
    private bool flattenSurface;

    [Header("Material settings")]
    public Material roadMaterial;
    public float textureTiling = 1;

    [SerializeField, HideInInspector]
    GameObject meshHolder;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    Mesh mesh;

    protected override void PathUpdated()
    {
        if (!autoUpdate)
            return;

        if (pathCreator != null)
        {
            AssignMeshComponents();
            AssignMaterials();
            CreateTrackMesh();
        }
    }

    /// <summary>
    /// Generates the track mesh based on the current path.
    /// </summary>
    private void CreateTrackMesh()
    {
        Vector3[] verts = new Vector3[path.NumPoints * 8];
        Vector2[] uvs = new Vector2[verts.Length];
        Vector3[] normals = new Vector3[verts.Length];

        int numTris = 2 * (path.NumPoints - 1) + ((path.isClosedLoop) ? 2 : 0);
        int[] roadTriangles = new int[numTris * 3];
        int[] underRoadTriangles = new int[numTris * 3];
        int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

        int vertIndex = 0;
        int triIndex = 0;

        int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
        int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

        bool usePathNormals = !(path.space == PathSpace.xyz && flattenSurface);

        for (int i = 0; i < path.NumPoints; i++)
        {
            AddRoadVertices(verts, uvs, normals, vertIndex, usePathNormals, i);
            SetTriangleIndices(verts, roadTriangles, underRoadTriangles, sideOfRoadTriangles, ref vertIndex, ref triIndex, triangleMap, sidesTriangleMap, i);
        }

        SetMeshProperties(verts, uvs, normals, roadTriangles, underRoadTriangles, sideOfRoadTriangles);
    }

    /// <summary>
    /// Adds the vertices for the road track.
    /// </summary>
    private void AddRoadVertices(Vector3[] verts, Vector2[] uvs, Vector3[] normals, int vertIndex, bool usePathNormals, int i)
    {
        Vector3 localUp = (usePathNormals) ? Vector3.Cross(path.GetTangent(i), path.GetNormal(i)) : path.up;
        Vector3 localRight = (usePathNormals) ? path.GetNormal(i) : Vector3.Cross(localUp, path.GetTangent(i));

        // Find position to left and right of current path vertex
        Vector3 vertSideA = path.GetPoint(i) - localRight;
        Vector3 vertSideB = path.GetPoint(i) + localRight * Mathf.Abs(roadHeight);

        // Add top of road vertices
        verts[vertIndex + 0] = vertSideA + localUp * (roadWidth / 2);
        verts[vertIndex + 1] = vertSideB + localUp * (roadWidth / 2);
        // Add bottom of road vertices
        verts[vertIndex + 2] = vertSideA - localUp * (roadWidth / 2);
        verts[vertIndex + 3] = vertSideB - localUp * (roadWidth / 2);

        // Duplicate vertices to get flat shading for sides of road
        verts[vertIndex + 4] = verts[vertIndex + 0];
        verts[vertIndex + 5] = verts[vertIndex + 1];
        verts[vertIndex + 6] = verts[vertIndex + 2];
        verts[vertIndex + 7] = verts[vertIndex + 3];

        // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
        uvs[vertIndex + 0] = new Vector2(0, path.times[i]);
        uvs[vertIndex + 1] = new Vector2(1, path.times[i]);

        // Top of road normals
        normals[vertIndex + 0] = localUp;
        normals[vertIndex + 1] = localUp;
        // Bottom of road normals
        normals[vertIndex + 2] = -localUp;
        normals[vertIndex + 3] = -localUp;
        // Sides of road normals
        normals[vertIndex + 4] = -localRight;
        normals[vertIndex + 5] = localRight;
        normals[vertIndex + 6] = -localRight;
        normals[vertIndex + 7] = localRight;
    }

    /// <summary>
    /// Sets the triangle indices for the road track.
    /// </summary>
    private void SetTriangleIndices(Vector3[] verts, int[] roadTriangles, int[] underRoadTriangles, int[] sideOfRoadTriangles, ref int vertIndex, ref int triIndex, int[] triangleMap, int[] sidesTriangleMap, int i)
    {
        // Set triangle indices
        if (i < path.NumPoints - 1 || path.isClosedLoop)
        {
            for (int j = 0; j < triangleMap.Length; j++)
            {
                roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
            }
            for (int j = 0; j < sidesTriangleMap.Length; j++)
            {
                sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
            }
        }

        vertIndex += 8;
        triIndex += 6;
    }

    /// <summary>
    /// Sets the properties of the mesh.
    /// </summary>
    private void SetMeshProperties(Vector3[] verts, Vector2[] uvs, Vector3[] normals, int[] roadTriangles, int[] underRoadTriangles, int[] sideOfRoadTriangles)
    {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.subMeshCount = 3;
        mesh.SetTriangles(roadTriangles, 0);
        mesh.SetTriangles(underRoadTriangles, 1);
        mesh.SetTriangles(sideOfRoadTriangles, 2);
        mesh.RecalculateBounds();
    }

    /// <summary>
    /// Assigns MeshRenderer and MeshFilter components to the mesh holder game object if not already attached.
    /// </summary>
    private void AssignMeshComponents()
    {
        if (meshHolder == null)
        {
            meshHolder = new GameObject("Road Mesh Holder");
        }

        meshHolder.transform.rotation = Quaternion.identity;
        meshHolder.transform.position = Vector3.zero;
        meshHolder.transform.localScale = Vector3.one;

        // Ensure mesh renderer, collider, and filter components are assigned
        if (!meshHolder.gameObject.GetComponent<MeshFilter>())
        {
            meshHolder.gameObject.AddComponent<MeshFilter>();
        }
        if (!meshHolder.GetComponent<MeshRenderer>())
        {
            meshHolder.gameObject.AddComponent<MeshRenderer>();
        }
        if (!meshHolder.GetComponent<MeshCollider>())
        {
            meshHolder.gameObject.AddComponent<MeshCollider>();
        }

        meshRenderer = meshHolder.GetComponent<MeshRenderer>();
        meshFilter = meshHolder.GetComponent<MeshFilter>();
        meshCollider = meshHolder.GetComponent<MeshCollider>();
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    /// <summary>
    /// Assigns the road material to the mesh renderer.
    /// </summary>
    private void AssignMaterials()
    {
        if (roadMaterial != null)
        {
            meshRenderer.sharedMaterials = new Material[] { roadMaterial, roadMaterial, roadMaterial };
            meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTiling);
        }
    }
}
