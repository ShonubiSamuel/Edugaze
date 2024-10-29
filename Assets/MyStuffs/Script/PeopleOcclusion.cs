
using UnityEngine;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(ARCameraManager))]
public class PeopleOcclusion : MonoBehaviour
{
    [SerializeField] 
    private XROrigin m_arOrigin = null;
    
    [SerializeField] 
    private AROcclusionManager occlusionManager = null;
    
    [SerializeField] 
    private ARCameraManager m_cameraManager = null;
    
    [SerializeField] 
    private Shader m_peopleOcclusionShader = null;
    
    //[SerializeField]
    //private Button nextTextureButton;

    [SerializeField] 
    private Texture2D[] humanOverlayTextures;

    private int currentTexture = 0;
    
    private Texture2D m_cameraFeedTexture = null;

  [SerializeField]
  private Material m_material;

	void Awake()
	{
       // m_material = new Material(m_peopleOcclusionShader);
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
      //  nextTextureButton.onClick.AddListener(NextTexture);
    }

    private void OnEnable()
    {
        m_cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        m_cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void NextTexture()
    {
        if(currentTexture == humanOverlayTextures.Length - 1)
            currentTexture = 0;
        else
            currentTexture++;

      //  nextTextureButton.GetComponentInChildren<Text>().text = $"Next Texture ({currentTexture})";
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
    Debug.Log("OnRenderImage called");
    if (PeopleOcclusionSupported())
        {
            if(m_cameraFeedTexture != null)
            {
                m_material.SetFloat("_UVMultiplierLandScape", CalculateUVMultiplierLandScape(m_cameraFeedTexture));
                m_material.SetFloat("_UVMultiplierPortrait", CalculateUVMultiplierPortrait(m_cameraFeedTexture));
            }
            
            if(Input.deviceOrientation == DeviceOrientation.LandscapeRight) 
            {
                m_material.SetFloat("_UVFlip", 0);
                m_material.SetInt("_ONWIDE", 1);
            }
            else if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft) 
            {
                m_material.SetFloat("_UVFlip", 1);
                m_material.SetInt("_ONWIDE", 1);
            }
            else
            {
                m_material.SetInt("_ONWIDE", 0);
            }

            m_material.SetTexture("_OcclusionDepth", occlusionManager.humanDepthTexture);
            m_material.SetTexture("_OcclusionStencil", occlusionManager.humanStencilTexture);
            Graphics.Blit(source, destination, m_material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if(PeopleOcclusionSupported())
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
       
    if (!m_cameraManager.TryAcquireLatestCpuImage(out XRCpuImage cameraImage))
    {
      return;
    }


        if (m_cameraFeedTexture == null || m_cameraFeedTexture.width != cameraImage.width || m_cameraFeedTexture.height != cameraImage.height)
        {
            m_cameraFeedTexture = new Texture2D(cameraImage.width, cameraImage.height, TextureFormat.RGBA32, false);
        }

        var imageTransformation = Input.deviceOrientation == DeviceOrientation.LandscapeRight ? XRCpuImage.Transformation.MirrorY : XRCpuImage.Transformation.MirrorX;
        var conversionParams = new XRCpuImage.ConversionParams(cameraImage, TextureFormat.RGBA32, imageTransformation);

        NativeArray<byte> rawTextureData = m_cameraFeedTexture.GetRawTextureData<byte>();

        try
        {
            unsafe
            {
                cameraImage.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
            }
        }
        finally
        {
            cameraImage.Dispose();
        }

        m_cameraFeedTexture.Apply();
        m_material.SetTexture("_CameraFeed", humanOverlayTextures[currentTexture]);


  }

  private float CalculateUVMultiplierLandScape(Texture2D cameraTexture)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraTextureAspect = (float)cameraTexture.width / (float)cameraTexture.height;
        return screenAspect / cameraTextureAspect;

    }
    private float CalculateUVMultiplierPortrait(Texture2D cameraTexture)
    {
        float screenAspect = (float)Screen.height / (float)Screen.width;
        float cameraTextureAspect = (float)cameraTexture.width / (float)cameraTexture.height;
        return screenAspect / cameraTextureAspect;
    }
}