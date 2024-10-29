using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;
using System;

[RequireComponent(typeof(ARCameraManager))]
public class PeopleOcclusionNormalMap : MonoBehaviour
{
  [SerializeField]
  private XROrigin m_arOrigin = null;

  [SerializeField]
  private AROcclusionManager occlusionManager = null;

  [SerializeField]
  private ARCameraManager m_cameraManager = null;

  [SerializeField]
  private Shader m_worldNormalShader = null;

  private Texture2D m_cameraFeedTexture = null;
  public Material m_material;

  private Mesh m_quadMesh;
  private Camera m_camera;

  void Awake()
  {
    m_camera = GetComponent<Camera>();
    m_quadMesh = CreateQuadMesh();
  }

  private void OnEnable()
  {
    m_cameraManager.frameReceived += OnCameraFrameReceived;
  }

  private void OnDisable()
  {
    m_cameraManager.frameReceived -= OnCameraFrameReceived;
  }

  void Update()
  {
    if (PeopleOcclusionSupported())
    {
      if (m_cameraFeedTexture != null)
      {
        m_material.SetFloat("_UVMultiplierLandScape", CalculateUVMultiplierLandScape(m_cameraFeedTexture));
        m_material.SetFloat("_UVMultiplierPortrait", CalculateUVMultiplierPortrait(m_cameraFeedTexture));
      }

      if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
      {
        m_material.SetFloat("_UVFlip", 0);
        m_material.SetInt("_ONWIDE", 1);
      }
      else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
      {
        m_material.SetFloat("_UVFlip", 1);
        m_material.SetInt("_ONWIDE", 1);
      }
      else
      {
        m_material.SetInt("_ONWIDE", 0);
      }

      if (occlusionManager.humanDepthTexture != null)
      {
        m_material.SetTexture("_HumanDepthTex", occlusionManager.humanDepthTexture);
        m_material.SetTexture("_OcclusionStencil", occlusionManager.humanStencilTexture);
      }

      // Pass the inverse view projection matrix to the shader
      Matrix4x4 projMatrix = GL.GetGPUProjectionMatrix(m_camera.projectionMatrix, false);
      Matrix4x4 viewMatrix = m_camera.worldToCameraMatrix;
      Matrix4x4 viewProjMatrix = projMatrix * viewMatrix;
      m_material.SetMatrix("_InvViewProjMatrix", viewProjMatrix.inverse);

      // Pass the texel size of the human depth texture
      m_material.SetVector("_HumanDepthTex_TexelSize", new Vector4(1.0f / occlusionManager.humanDepthTexture.width, 1.0f / occlusionManager.humanDepthTexture.height, 0, 0));

      // Draw the quad
      Graphics.DrawMesh(m_quadMesh, Matrix4x4.identity, m_material, 0);
    }
  }

  private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
  {
    if (PeopleOcclusionSupported())
    {
      RefreshCameraFeedTexture();
    }
  }

  private bool PeopleOcclusionSupported()
  {
    return occlusionManager.subsystem != null && occlusionManager.humanDepthTexture != null && occlusionManager.humanStencilTexture != null;
  }

  private void RefreshCameraFeedTexture()
  {
    // If needed, uncomment and implement CPU image acquisition and conversion
  }

  private float CalculateUVMultiplierLandScape(Texture2D cameraTexture)
  {
    // Placeholder calculation
    return 0.346564f;
  }

  private float CalculateUVMultiplierPortrait(Texture2D cameraTexture)
  {
    // Placeholder calculation
    return 1.623077f;
  }

  private Mesh CreateQuadMesh()
  {
    Mesh mesh = new Mesh();

    Vector3[] vertices = new Vector3[4]
    {
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, 1, 0)
    };

    int[] tris = new int[6]
    {
            0, 2, 1,
            0, 3, 2
    };

    Vector3[] normals = new Vector3[4]
    {
            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
            Vector3.forward
    };

    Vector2[] uv = new Vector2[4]
    {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
    };

    mesh.vertices = vertices;
    mesh.triangles = tris;
    mesh.normals = normals;
    mesh.uv = uv;

    return mesh;
  }
}
