using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerFollowCamera : MonoBehaviour
{
  public Camera targetCamera;  // The camera to follow
  public Vector3 positionOffset; // The offset from the camera
  public Vector3 rotationOffset; // The rotation offset in degrees
  public float smoothSpeed = 0.125f; // Speed of the smoothing for position
  public float rotationSpeed = 5.0f; // Speed of the smoothing for rotation

  public static VideoPlayerFollowCamera Instance;

  GameObject videoPlayer;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
  }

  public bool shouldFollow;

  void Update()
  {
    if (shouldFollow)
    {
      if (targetCamera == null)
      {
        targetCamera = Camera.main;
      }
      if (videoPlayer == null)
      {
        try
        {
          videoPlayer = FindObjectOfType<VideoPlayer>().gameObject;

          videoPlayer.GetComponent<VideoPlayer>().Play();

          VoiceCommands.instance.Deactivate();
        }
        finally
        {

        }
      }

      if (videoPlayer == null)
      {
        Debug.LogError("Video player is null");
        return;
      }

      // Calculate the desired position in front of the camera with the given position offset
      Vector3 desiredPosition = targetCamera.transform.position + targetCamera.transform.forward * positionOffset.z + targetCamera.transform.right * positionOffset.x + targetCamera.transform.up * positionOffset.y;

      // Smoothly interpolate between the current position and the desired position
      videoPlayer.transform.position = Vector3.Lerp(videoPlayer.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

      // Calculate the desired rotation to look at the camera
      Quaternion desiredRotation = Quaternion.LookRotation(targetCamera.transform.position - videoPlayer.transform.position);

      // Apply rotation offset
      desiredRotation *= Quaternion.Euler(rotationOffset);

      // Smoothly rotate towards the desired rotation
      videoPlayer.transform.rotation = Quaternion.RotateTowards(videoPlayer.transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime * 360);
    }
  }
}
