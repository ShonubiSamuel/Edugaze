using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static UnityEngine.SpatialTracking.TrackedPoseDriver;

public class ProjectorBehaviour : MonoBehaviour
{
    [SerializeField]
    protected Material spotMaterial;

    AROcclusionManager occlusionManager;
    ARCameraManager cameraManager;

    Matrix4x4 _UnityDisplayTransform = Matrix4x4.identity;

    LineRenderer lineRenderer;

    public void Start()
    {
        // To retrieve the UnityDisplayTranssformation Matrix to apply to the Environment depth UVs
        cameraManager = Camera.main.transform.GetComponent<ARCameraManager>();
        cameraManager.frameReceived += Native_FrameReceived;

        // To retrieve the EnvironmentDepth texture to calculate the world position of the Camera pixels => LiDAR magic!
        occlusionManager = Camera.main.transform.GetComponent<AROcclusionManager>();

        lineRenderer = GetComponent<LineRenderer>();
    }

    // to retreive the AR Camera matrix
    private void Native_FrameReceived(ARCameraFrameEventArgs _arg)
    {
        if (_arg != null)
        {
            if (_arg.displayMatrix.HasValue)
            {
                _UnityDisplayTransform = _arg.displayMatrix.Value;
            }
        }
    }

    private void Update()
    {
        // Reference both variables to be used for our shader
        spotMaterial.SetTexture("_EnvironmentDepth", occlusionManager.environmentDepthTexture);
        spotMaterial.SetMatrix("_UnityDisplayTransform", _UnityDisplayTransform);


        lineRenderer.SetPosition(0, Camera.main.transform.position + Camera.main.transform.forward / 2);

        // raycast between the phone position and the playspace (might need a few secons do be captured)
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
        {
            print("it hitts");
            spotMaterial.SetVector("_Target", hit.point);
            lineRenderer.SetPosition(1, hit.point);
        }
    }
}

