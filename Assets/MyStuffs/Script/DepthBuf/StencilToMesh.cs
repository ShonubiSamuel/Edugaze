using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HumanStencilTextureProcessor : MonoBehaviour
{
  public AROcclusionManager occlusionManager;  // Reference to the AR Occlusion Manager
  public GameObject displayerComponent;        // GameObject for displaying sprite
  public GameObject mesher;                    // GameObject with MeshFilter and MeshCollider components

  private Texture2D stencilTexture;            // Texture2D generated from human stencil
  private Texture2D processedTexture;          // Processed texture with transparency applied
  public SpriteRenderer spriteRenderer;        // SpriteRenderer to display the processed texture
  private PolygonCollider2D polygonColliderAdded; // Collider to get shape information

  private Mesh meshFilterMesh;                 // Mesh used for MeshFilter
  private List<Vector3> vertices;              // List to hold mesh vertices
  private List<int> triangles;                 // List to hold mesh triangle indices
  private int loop = 0;                        // Counter for vertices/triangle creation

  [SerializeField]
  [Tooltip("The ARCameraManager which will produce frame events.")]
  ARCameraManager m_CameraManager;

  void Start()
  {
    // Initialize lists and mesh data structures
    vertices = new List<Vector3>();
    triangles = new List<int>();

    // Add a SpriteRenderer component to the GameObject
    //spriteRenderer = displayerComponent.AddComponent<SpriteRenderer>();

    // Initialize textures
    stencilTexture = new Texture2D(1, 1);   // Dummy initialization, will be replaced later
    processedTexture = new Texture2D(1, 1);

    // Get the mesh components
    meshFilterMesh = new Mesh();
    mesher.GetComponent<MeshFilter>().mesh = meshFilterMesh;
    mesher.GetComponent<MeshCollider>().sharedMesh = meshFilterMesh;
  }

  void OnEnable()
  {
    if (m_CameraManager == null)
    {
      return;
    }

    m_CameraManager.frameReceived += Updates;
  }

  void OnDisable()
  {
    if (m_CameraManager != null)
    {
      m_CameraManager.frameReceived -= Updates;
    }
  }

  void Updates(ARCameraFrameEventArgs eventArgs)
  {
    // Try to acquire the human stencil CPU image
    if (occlusionManager.TryAcquireHumanStencilCpuImage(out XRCpuImage cpuImage))
    {
      // Convert the XRCpuImage to Texture2D
      stencilTexture = ConvertCpuImageToTexture2D(cpuImage);
      cpuImage.Dispose();  // Dispose of the XRCpuImage to free resources

      // Process the texture to make white pixels transparent
      processedTexture = ProcessStencilTexture(stencilTexture);

      // Create and assign a sprite to the SpriteRenderer for visualization
      Sprite sprite = Sprite.Create(processedTexture, new Rect(0.0f, 0.0f, processedTexture.width, processedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
      spriteRenderer.sprite = sprite;

      // Add the polygon collider to our displayerComponent and get its path count
      polygonColliderAdded = displayerComponent.GetComponent<PolygonCollider2D>();
      if (polygonColliderAdded == null)
      {
        polygonColliderAdded = displayerComponent.AddComponent<PolygonCollider2D>();
      }

      // Generate the mesh from the collider
      BrowseColliderToCreateMesh(polygonColliderAdded);

      Debug.Log("Mesh generated.");
    }
    else
    {
      Debug.LogWarning("Failed to acquire human stencil image.");
    }
  }

  unsafe private Texture2D ConvertCpuImageToTexture2D(XRCpuImage cpuImage)
  {
    // Create a Texture2D with the appropriate format
    Texture2D texture = new Texture2D(cpuImage.width, cpuImage.height, TextureFormat.R8, false);

    // Define conversion parameters
    var conversionParams = new XRCpuImage.ConversionParams
    {
      inputRect = new RectInt(0, 0, cpuImage.width, cpuImage.height),
      outputDimensions = new Vector2Int(cpuImage.width, cpuImage.height),
      outputFormat = TextureFormat.R8,  // Use single-channel R8 format
      transformation = XRCpuImage.Transformation.None
    };

    // Allocate a buffer for the converted image data
    var rawTextureData = new NativeArray<byte>(cpuImage.GetConvertedDataSize(conversionParams), Allocator.Temp);

    // Perform the image conversion
    cpuImage.Convert(conversionParams, new System.IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);

    // Load the raw image data into the texture
    texture.LoadRawTextureData(rawTextureData);
    texture.Apply();

    // Dispose of the native array to free memory
    rawTextureData.Dispose();

    return texture;
  }

  private Texture2D ProcessStencilTexture(Texture2D texture)
  {
    // Create a new Texture2D for the processed output
    Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
    Color[] pixels = texture.GetPixels();
    Color[] newPixels = new Color[pixels.Length];

    // Convert the stencil data to a usable texture format
    for (int i = 0; i < pixels.Length; i++)
    {
      // Assume that non-zero R channel indicates human presence (white pixels)
      if (pixels[i].r > 0.5f)
      {
        newPixels[i] = Color.white;  // Keep white for visible human parts
      }
      else
      {
        newPixels[i] = Color.clear;  // Make the rest transparent
      }
    }

    // Apply the new pixel data to the new texture
    newTexture.SetPixels(newPixels);
    newTexture.Apply();

    return newTexture;
  }

  public void BrowseColliderToCreateMesh(PolygonCollider2D polygonColliderAdded)
  {
    // Clear previous data
    vertices.Clear();
    triangles.Clear();
    loop = 0;

    // Browse all paths from collider
    int pathCount = polygonColliderAdded.pathCount;
    for (int i = 0; i < pathCount; i++)
    {
      Vector2[] path = polygonColliderAdded.GetPath(i);

      // Browse all path points
      for (int j = 1; j < path.Length; j++)
      {
        Vector3 point0 = new Vector3(path[j - 1].x, path[j - 1].y, 0);
        Vector3 point1 = new Vector3(path[j - 1].x, path[j - 1].y, 10f);  // Assuming size is 10 units in depth
        Vector3 point2 = new Vector3(path[j].x, path[j].y, 10f);
        Vector3 point3 = new Vector3(path[j].x, path[j].y, 0);

        MakeMesh(point0, point1, point2, point3);

        if (j == path.Length - 1) // If we are at the last point, close the loop with the first point
        {
          Vector3 point0Closing = new Vector3(path[j].x, path[j].y, 0);
          Vector3 point1Closing = new Vector3(path[j].x, path[j].y, 10f);
          Vector3 point2Closing = new Vector3(path[0].x, path[0].y, 10f); // First point
          Vector3 point3Closing = new Vector3(path[0].x, path[0].y, 0);   // First point

          MakeMesh(point0Closing, point1Closing, point2Closing, point3Closing);
        }
      }
    }

    // Update the mesh with new vertices and triangles
    meshFilterMesh.Clear();
    meshFilterMesh.vertices = vertices.ToArray();
    meshFilterMesh.triangles = triangles.ToArray();
    meshFilterMesh.RecalculateNormals();

    // Update the mesh collider with the new mesh
    mesher.GetComponent<MeshCollider>().sharedMesh = meshFilterMesh;
  }

  public void MakeMesh(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
  {
    // Vertice add
    vertices.Add(point0);
    vertices.Add(point1);
    vertices.Add(point2);
    vertices.Add(point3);

    // Triangle order
    triangles.Add(0 + loop * 4);
    triangles.Add(2 + loop * 4);
    triangles.Add(1 + loop * 4);
    triangles.Add(0 + loop * 4);
    triangles.Add(3 + loop * 4);
    triangles.Add(2 + loop * 4);

    loop++;
  }
}
