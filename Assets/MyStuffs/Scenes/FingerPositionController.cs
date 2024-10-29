using UnityEngine;

public class FingerMovement : MonoBehaviour
{
  public Transform middleMcp;  // Assign in Inspector
  public Transform wrist;      // Assign in Inspector
  public Transform thumbMcp;   // Assign in Inspector
  public Transform target;

  private Vector3 initialMiddleDirection;
  private Vector3 initialThumbDirection;
  private float initialMiddleDistance;
  private float initialThumbDistance;

  void Start()
  {
    // Calculate initial directions and distances
    initialMiddleDirection = middleMcp.position - wrist.position;
    initialMiddleDistance = initialMiddleDirection.magnitude;

    initialThumbDirection = thumbMcp.position - wrist.position;
    initialThumbDistance = initialThumbDirection.magnitude;

    // Normalize directions for initial checks
    initialMiddleDirection.Normalize();
    initialThumbDirection.Normalize();
  }

  void Update()
  {
    // Restrict middleMcp within initial distance
    Vector3 currentMiddleDirection = middleMcp.position - wrist.position;
    if (currentMiddleDirection.magnitude > initialMiddleDistance)
    {
      middleMcp.position = wrist.position + currentMiddleDirection.normalized * initialMiddleDistance;
    }

    // Restrict thumbMcp within initial distance
    Vector3 currentThumbDirection = thumbMcp.position - wrist.position;
    if (currentThumbDirection.magnitude > initialThumbDistance)
    {
      thumbMcp.position = wrist.position + currentThumbDirection.normalized * initialThumbDistance;
    }

    // Calculate directions for roll and yaw as before
    Vector3 middleDirection = middleMcp.position - wrist.position;
    middleDirection.Normalize();

    Vector3 thumbDirection = thumbMcp.position - wrist.position;
    thumbDirection.Normalize();

    // Calculate yaw angle
    float yaw = Mathf.Atan2(middleDirection.x, middleDirection.z) * Mathf.Rad2Deg;

    // Calculate roll angle
    float roll = -Mathf.Atan2(thumbDirection.x, thumbDirection.y) * Mathf.Rad2Deg - 90;

    // Calculate pitch angle
    float pitch = -Mathf.Atan2(middleDirection.y, middleDirection.z) * Mathf.Rad2Deg;

    // Create rotations for pitch, yaw, and roll
    Quaternion pitchRotation = Quaternion.AngleAxis(pitch, Vector3.right);
    Quaternion yawRotation = Quaternion.AngleAxis(yaw, Vector3.up);
    Quaternion rollRotation = Quaternion.AngleAxis(roll, Vector3.forward);

    // Combine rotations: apply yaw, then pitch, then roll
    Quaternion targetRotation = yawRotation * pitchRotation * rollRotation;

    // Apply the new rotation to the target
    target.rotation = targetRotation;

    //Debug.Log("Pitch: " + pitch + " | Yaw: " + yaw + " | Roll: " + roll);
  }

}
