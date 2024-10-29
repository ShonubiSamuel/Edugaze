using UnityEngine;

public class FingerRotations : MonoBehaviour
{
  public Transform[] indexJoints;  // Assign in Inspector: MCP, PIP, DIP, Tip for Index finger

  // Hand tracking data for the index finger
  public Transform handTrackingTip;
  public Transform handTrackingDistal;
  public Transform handTrackingInter;
  public Transform handTrackingProx;

  // Limits for joint rotations (in degrees)
  public Vector2[] rotationLimits;  // Define min and max for each joint (MCP, PIP, DIP)

  void Update()
  {
    // Update finger rotations based on hand tracking data
    UpdateFingerRotation(indexJoints, new Transform[] { handTrackingProx, handTrackingInter, handTrackingDistal, handTrackingTip });
  }

  // Function to update finger rotation based on hand tracking data
  void UpdateFingerRotation(Transform[] fingerJoints, Transform[] handTrackingPositions)
  {
    // Ensure the correct number of joints and tracking positions are assigned
    if (fingerJoints.Length != 4 || handTrackingPositions.Length != 4 || rotationLimits.Length != 3)
    {
      Debug.LogError("Incorrect number of finger joints, hand tracking positions, or rotation limits.");
      return;
    }

    // Calculate and apply rotations for each joint
    for (int i = 0; i < handTrackingPositions.Length - 1; i++)
    {
      // Get the direction from the current joint to the next joint
      Vector3 direction = handTrackingPositions[i + 1].position - handTrackingPositions[i].position;

      // For `LookRotation`, you need a forward and an up direction. Here, we'll use the direction as the forward direction,
      // and assume that the local up direction is along the world up vector.
      // Alternatively, you could use a vector perpendicular to the `direction` for a more realistic up direction.
      Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

      // Convert the rotation to Euler angles
      Vector3 eulerAngles = rotation.eulerAngles;

      // Clamp the pitch (X-axis rotation) for this joint
      float pitch = eulerAngles.x;
      //pitch = Mathf.Clamp(pitch, rotationLimits[i].x, rotationLimits[i].y);

      // Apply the clamped rotation to the finger joint
      fingerJoints[i].rotation = Quaternion.Euler(pitch, 0, 0) * fingerJoints[i].rotation;

      // Print the pitch value specifically for index 2
      if (i == 2)
      {
        Debug.Log($"Joint {i} Pitch: {pitch:F3}");
      }
    }
  }
}
