using UnityEngine;

//public class RotationByPitchAndRoll : MonoBehaviour
//{
//  public GameObject wrist;    // First GameObject (Wrist)
//  public GameObject ring0;    // Second GameObject (Ring Finger)
//  public GameObject thumb0;   // Third GameObject (Thumb)
//  public GameObject targetObject; // The object to set the rotation

//  public float smoothSpeed = 5.0f; // Speed of the smooth rotation

//  private Quaternion initialRotation;

//  void Start()
//  {
//    // Store the initial rotation of the target object
//    initialRotation = targetObject.transform.rotation;
//  }

//  void Update()
//  {
//    // Calculate direction vectors
//    Vector3 directionRing = ring0.transform.position - wrist.transform.position;
//    Vector3 directionThumb = thumb0.transform.position - wrist.transform.position;

//    // Calculate the angle for x-axis rotation (pitch) using the direction from wrist to ring0
//    float angleRing = Mathf.Atan2(directionRing.z, directionRing.y) * Mathf.Rad2Deg;

//    // Calculate the angle for z-axis rotation (roll) using the direction from wrist to thumb0
//    float angleThumb = Mathf.Atan2(directionThumb.y, directionThumb.x) * Mathf.Rad2Deg;

//    // Calculate the target rotation relative to the initial rotation
//    Quaternion targetRotation = initialRotation * Quaternion.Euler(angleRing, 0, angleThumb);

//    // Smoothly interpolate to the target rotation
//    targetObject.transform.rotation = Quaternion.Lerp(
//        targetObject.transform.rotation,
//        targetRotation,
//        Time.deltaTime * smoothSpeed
//    );
//  }
//}


using UnityEngine;

/// <summary>
/// Rotates the target object based on the positions of the wrist, ring finger, and thumb GameObjects.
/// The rotation is smoothed using spherical interpolation (Slerp).
/// </summary>
public class RotationByLandmarkDirection : MonoBehaviour
{
  [Header("Landmark GameObjects")]
  [Tooltip("GameObject representing the wrist.")]
  public GameObject wrist;

  [Tooltip("GameObject representing the ring finger.")]
  public GameObject ringFinger;

  [Tooltip("GameObject representing the thumb.")]
  public GameObject thumb;

  [Tooltip("GameObject representing the ring finger.")]
  public GameObject index0;

  [Tooltip("GameObject representing the thumb.")]
  public GameObject middle0;

  [Tooltip("GameObject representing the thumb.")]
  public GameObject pinky0;

  [Header("Target Object")]
  [Tooltip("The object to set the rotation.")]
  public GameObject targetObject;

  [Header("Rotation Settings")]
  [Tooltip("Speed of the smooth rotation.")]
  [Range(0.1f, 10.0f)]
  public float smoothSpeed = 5.0f;

  /// <summary>
  /// Updates the rotation of the target object every frame based on the landmarks.
  /// </summary>
  //private void Update()
  //{
  //  if (wrist == null || ringFinger == null || thumb == null || targetObject == null)
  //  {
  //    Debug.LogError("One or more required GameObjects are not assigned.");
  //    return;
  //  }

  //  // Calculate the direction vectors from the wrist to the ring finger and thumb.
  //  Vector3 forwardDirection = (ringFinger.transform.position - wrist.transform.position).normalized;
  //  Vector3 upDirection = (pinky0.transform.position - index0.transform.position).normalized;

  //  // Ensure the vectors are valid and not parallel to avoid unexpected behavior.
  //  if (forwardDirection.sqrMagnitude > 0.0f && upDirection.sqrMagnitude > 0.0f &&
  //      Vector3.Cross(forwardDirection, upDirection).sqrMagnitude > Mathf.Epsilon)
  //  {
  //    // Calculate the target rotation using LookRotation.
  //    Quaternion targetRotation = Quaternion.LookRotation(forwardDirection, upDirection);

  //    // Smoothly interpolate the rotation using Slerp.
  //    targetObject.transform.rotation = Quaternion.Slerp(
  //        targetObject.transform.rotation,
  //        targetRotation,
  //        Time.deltaTime * smoothSpeed
  //    );

  //    // Debugging lines to visualize directions.
  //    Debug.DrawLine(wrist.transform.position, ringFinger.transform.position, Color.red);
  //    Debug.DrawLine(index0.transform.position, pinky0.transform.position, Color.green);
  //  }
  //  else
  //  {
  //    Debug.LogWarning("The direction vectors are invalid or nearly parallel. Rotation will not be applied.");
  //  }
  //}


  // Assuming we have an array of Vector3 representing the 3D positions of the 21 landmarks
    public Vector3[] handLandmarks;

    void Update()
    {
    CalculateHandRotation();
  }

  void CalculateHandRotation()
  {
    // Calculate the direction vector from the wrist to the middle finger base
    Vector3 wristToMiddleFinger = middle0.transform.position - wrist.transform.position;

    // Pitch Calculation (YZ plane)
    float pitch = Mathf.Atan2(wristToMiddleFinger.y, wristToMiddleFinger.z) * Mathf.Rad2Deg;

    // Yaw Calculation (XZ plane)
    float yaw = Mathf.Atan2(wristToMiddleFinger.x, wristToMiddleFinger.z) * Mathf.Rad2Deg;

    // Roll Calculation (XY plane) using thumb and pinky base landmarks
    Vector3 thumbToPinky = pinky0.transform.position - thumb.transform.position;
    float roll = Mathf.Atan2(thumbToPinky.y, thumbToPinky.x) * Mathf.Rad2Deg;

    // Apply rotation to a GameObject (for example)
    targetObject.transform.rotation = Quaternion.Euler(-pitch, yaw, -roll);

    Debug.Log($"Pitch: {pitch}, Yaw: {yaw}, Roll: {roll}");
  }


}
