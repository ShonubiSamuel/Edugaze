using System.Collections.Generic; // Required for using List
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DynamicPolygonMesh : MonoBehaviour
{
  private Mesh mesh;
  private List<Vector3> vertices = new List<Vector3>();
  private List<int> triangles = new List<int>();
  private List<Vector2> uv = new List<Vector2>();
  private List<Vector3> normals = new List<Vector3>();

  // List to hold reference points that determine the polygon's vertices
  public List<Transform> points = new List<Transform>();

  void Start()
  {
    // Initialize mesh and attach it to the MeshFilter component
    mesh = new Mesh();
    GetComponent<MeshFilter>().mesh = mesh;
  }

  void Update()
  {
    // Ensure there are enough points to create at least one triangle
    if (points.Count < 3)
    {
      Debug.LogError("At least 3 points are required to form a polygon.");
      return;
    }

    // Clear existing data
    vertices.Clear();
    triangles.Clear();
    uv.Clear();
    normals.Clear();

    // Update vertices based on the positions of the reference points
    for (int i = 0; i < points.Count; i++)
    {
      // Convert world position to local position
      vertices.Add(transform.InverseTransformPoint(points[i].position));

      // Normals pointing forward
      normals.Add(-Vector3.forward);

      // UV mapping (simple placeholder, you may need to adjust based on your texture mapping needs)
      uv.Add(new Vector2(vertices[i].x, vertices[i].y));
    }

    // Create triangles (triangulation)
    for (int i = 1; i < points.Count - 1; i++)
    {
      triangles.Add(0);
      triangles.Add(i);
      triangles.Add(i + 1);
    }

    // Clear and update mesh data
    mesh.Clear();
    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.uv = uv.ToArray();
    mesh.normals = normals.ToArray();

    // Recalculate normals for lighting (optional if normals are not dynamic)
    mesh.RecalculateNormals();
  }
}
