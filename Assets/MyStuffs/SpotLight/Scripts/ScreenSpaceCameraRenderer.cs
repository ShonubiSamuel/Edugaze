using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#nullable disable

/// <summary>
/// A render feature for rendering the camera background for AR devices.
/// </summary>
public class ScreenSpaceCameraRenderer : ScriptableRendererFeature
{
    [SerializeField]
    public Material customMaterial;
    [SerializeField]
    RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
    [SerializeField]
    bool invertCulling;

    [SerializeField]
    bool editorOnly = false;

    /// <summary>
    /// The scriptable render pass to be added to the renderer when the camera background is to be rendered.
    /// </summary>
    CustomRenderPass m_ScriptablePass;

    /// <summary>
    /// The mesh for rendering the background shader.
    /// </summary>
    Mesh m_BackgroundMesh;

    /// <summary>
    /// Create the scriptable render pass.
    /// </summary>
    public override void Create()
    {
#if !UNITY_EDITOR
        if (editorOnly)
            return;
#endif
        m_ScriptablePass = new CustomRenderPass(renderPassEvent);

        m_BackgroundMesh = new Mesh();
        m_BackgroundMesh.vertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0.1f),
            new Vector3(0f, 1f, 0.1f),
            new Vector3(1f, 1f, 0.1f),
            new Vector3(1f, 0f, 0.1f),
        };
        m_BackgroundMesh.uv = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
        };
        m_BackgroundMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    }

    /// <summary>
    /// Add the background rendering pass when rendering a game camera with an enabled AR camera background component.
    /// </summary>
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if !UNITY_EDITOR
        if (editorOnly)
            return;
#endif
        Camera currentCamera = renderingData.cameraData.camera;
        if (currentCamera != null && currentCamera.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }

    /// <summary>
    /// Setup the render passes when render targets are allocated.
    /// </summary>
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        Camera currentCamera = renderingData.cameraData.camera;
        if (currentCamera != null && currentCamera.cameraType == CameraType.Game)
        {
            // Using RTHandle for color and depth targets
            m_ScriptablePass.Setup(m_BackgroundMesh, customMaterial, invertCulling,
                                   renderer.cameraColorTargetHandle, null);
        }
    }

    /// <summary>
    /// The custom render pass to render the camera background.
    /// </summary>
    class CustomRenderPass : ScriptableRenderPass
    {
        const string k_CustomRenderPassName = "XPAREditorCameraRenderer";
        static readonly Matrix4x4 k_BackgroundOrthoProjection = Matrix4x4.Ortho(0f, 1f, 0f, 1f, -0.1f, 9.9f);

        Mesh m_BackgroundMesh;
        Material m_BackgroundMaterial;
        RTHandle m_ColorTargetHandle;
        RTHandle m_DepthTargetHandle;
        bool m_InvertCulling;

        public CustomRenderPass(RenderPassEvent renderPassEvent)
        {
            this.renderPassEvent = renderPassEvent;
        }

        /// <summary>
        /// Setup the background render pass.
        /// </summary>
        public void Setup(Mesh backgroundMesh, Material backgroundMaterial, bool invertCulling,
                          RTHandle colorTargetHandle, RTHandle depthTargetHandle)
        {
            m_BackgroundMesh = backgroundMesh;
            m_BackgroundMaterial = backgroundMaterial;
            m_InvertCulling = invertCulling;
            m_ColorTargetHandle = colorTargetHandle;
            m_DepthTargetHandle = depthTargetHandle;
        }

        /// <summary>
        /// Configure the render pass by configuring the render target and clear values.
        /// </summary>
        public override void Configure(CommandBuffer commandBuffer, RenderTextureDescriptor renderTextureDescriptor)
        {
            // Configuration is not needed for now.
        }

        /// <summary>
        /// Execute the commands to render the camera background.
        /// </summary>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (m_BackgroundMesh == null)
            {
                return;
            }

            var cmd = CommandBufferPool.Get(k_CustomRenderPassName);
            cmd.BeginSample(k_CustomRenderPassName);

            cmd.SetInvertCulling(m_InvertCulling);

            cmd.SetViewProjectionMatrices(Matrix4x4.identity, k_BackgroundOrthoProjection);
            CoreUtils.SetRenderTarget(cmd, m_ColorTargetHandle, m_DepthTargetHandle, ClearFlag.Depth, Color.clear);
            cmd.DrawMesh(m_BackgroundMesh, Matrix4x4.identity, m_BackgroundMaterial);
            cmd.SetViewProjectionMatrices(renderingData.cameraData.camera.worldToCameraMatrix,
                                          renderingData.cameraData.camera.projectionMatrix);

            cmd.EndSample(k_CustomRenderPassName);
            context.ExecuteCommandBuffer(cmd);

            CommandBufferPool.Release(cmd);
        }

        /// <summary>
        /// Clean up any resources for the render pass.
        /// </summary>
        public override void FrameCleanup(CommandBuffer commandBuffer)
        {
            m_ColorTargetHandle?.Release();
            m_DepthTargetHandle?.Release();
        }
    }
}
