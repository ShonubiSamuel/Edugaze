using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;
using System;

[RequireComponent(typeof(ARCameraManager))]
public class DepthToMesh : MonoBehaviour
{
  [SerializeField]
  private XROrigin m_arOrigin = null;

  [SerializeField]
  private AROcclusionManager occlusionManager = null;

  [SerializeField]
  private ARCameraManager m_cameraManager = null;

  [SerializeField]
  private MeshFilter m_meshFilter = null;  // MeshFilter to assign the generated mesh

  private void OnEnable()
  {
    // Subscribe to camera frame updates
    m_cameraManager.frameReceived += OnCameraFrameReceived;
  }

  private void OnDisable()
  {
    // Unsubscribe from camera frame updates
    m_cameraManager.frameReceived -= OnCameraFrameReceived;
  }

  private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
  {
    // Generate mesh every frame if occlusion is supported and depth texture is available
    if (PeopleOcclusionSupported())
    {
      GenerateMeshFromDepth();
    }
  }

  private bool PeopleOcclusionSupported()
  {
    // Check if the occlusion manager and depth textures are available
    return occlusionManager.subsystem != null
        && occlusionManager.humanDepthTexture != null;
  }

  private void GenerateMeshFromDepth()
  {
    if (!occlusionManager.TryAcquireHumanDepthCpuImage(out XRCpuImage depthImage))
    {
      return;
    }

    // Ensure the image is disposed after use
    using (depthImage)
    {
      // Define the conversion parameters to convert depth image to grayscale float format
      var conversionParams = new XRCpuImage.ConversionParams
      {
        // Rect describes the portion of the image we want
        inputRect = new RectInt(0, 0, depthImage.width, depthImage.height),
        // Output dimensions should match the input
        outputDimensions = new Vector2Int(depthImage.width, depthImage.height),
        // Choose a format to convert to
        outputFormat = TextureFormat.RFloat, // Use RFloat for depth map
                                             // No transformation
        transformation = XRCpuImage.Transformation.None
      };

      // Calculate the size of the output buffer
      int size = depthImage.GetConvertedDataSize(conversionParams);

      // Allocate a buffer to store the converted data
      NativeArray<float> buffer = new NativeArray<float>(size / sizeof(float), Allocator.Temp);

      try
      {
        unsafe
        {
          // Convert the image
          depthImage.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length * sizeof(float));
        }
      }
      finally
      {
        // Create mesh from depth data
        CreateMeshFromDepthData(buffer, depthImage.width, depthImage.height);

        // Dispose the native array after use
        buffer.Dispose();
      }

      

     
    }
  }

  private void CreateMeshFromDepthData(NativeArray<float> depthData, int width, int height)
  {
    Mesh mesh = new Mesh();
    mesh.name = "DepthMesh";

    Vector3[] vertices = new Vector3[width * height];
    Vector2[] uvs = new Vector2[width * height];
    int[] triangles = new int[(width - 1) * (height - 1) * 6];

    int triIndex = 0;
    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        int index = y * width + x;
        float depth = depthData[index];

        // Set vertex position with normalized x, y, and depth as z-coordinate
        vertices[index] = new Vector3(x / (float)width, y / (float)height, depth);
        uvs[index] = new Vector2(x / (float)width, y / (float)height);

        // Create two triangles for each grid cell, except at the boundaries
        if (x < width - 1 && y < height - 1)
        {
          triangles[triIndex] = index;
          triangles[triIndex + 1] = index + width;
          triangles[triIndex + 2] = index + width + 1;

          triangles[triIndex + 3] = index;
          triangles[triIndex + 4] = index + width + 1;
          triangles[triIndex + 5] = index + 1;

          triIndex += 6;
        }
      }
    }

    // Assign vertices, UVs, and triangles to the mesh
    mesh.vertices = vertices;
    mesh.uv = uvs;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();

    // Assign the generated mesh to the MeshFilter to display it
    if (m_meshFilter != null)
    {
      m_meshFilter.mesh = mesh;
    }
  }
}
