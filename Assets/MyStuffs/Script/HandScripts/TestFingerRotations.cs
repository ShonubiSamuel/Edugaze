//using UnityEngine;


//public class FingerRotation : MonoBehaviour
//{
//  // Hand model joints added in the Inspector for each finger
//  public Transform[] indexJoints;  // Assign in Inspector: MCP, PIP, DIP, Tip for Index finger
//  public Transform[] middleJoints; // Assign in Inspector: MCP, PIP, DIP, Tip for Middle finger
//  public Transform[] ringJoints;   // Assign in Inspector: MCP, PIP, DIP, Tip for Ring finger
//  public Transform[] pinkyJoints;  // Assign in Inspector: MCP, PIP, DIP, Tip for Pinky finger
//  public Transform[] thumbJoints;  // Assign in Inspector: MCP, PIP, DIP, Tip for Thumb

//  public Transform[] testIndexJoint; // Test joints for hand tracking (for testing)

//  public Transform handParent;

//  void Update()
//  {
//    // Update rotations for all fingers based on hand tracking data
//    UpdateFingerRotation(indexJoints);   // Index finger
//                                         //UpdateFingerRotation(middleJoints);  // Middle finger
//                                         //UpdateFingerRotation(ringJoints);    // Ring finger
//                                         //UpdateFingerRotation(pinkyJoints);   // Pinky finger
//                                         //UpdateFingerRotation(thumbJoints);   // Thumb
//  }

//  public float minPitchClampMcp;
//  public float maxPitchClampMcp;

//  public float minYawClampMcp;
//  public float maxYawClampMcp;

//  public float minPitchClamp;
//  public float maxPitchClamp;

//  public float minYawClamp;
//  public float maxYawClamp;


//  // Function to update finger rotation based on hand tracking data
//  void UpdateFingerRotation(Transform[] fingerJoints)
//  {
//    for (int i = 0; i < fingerJoints.Length - 1; i++)
//    {
//      // Retrieve hand tracking positions for the given finger (MCP, PIP, DIP)
//      Vector3[] handTrackingPositions = new Vector3[4];
//      handTrackingPositions[0] = testIndexJoint[0].position;  // MCP
//      handTrackingPositions[1] = testIndexJoint[1].position;  // PIP
//      handTrackingPositions[2] = testIndexJoint[2].position;  // DIP
//      handTrackingPositions[3] = testIndexJoint[3].position;  // Tip

//      // Calculate pitch and yaw, and apply rotations to the hand model
//      Vector3 direction = handTrackingPositions[i + 1] - handTrackingPositions[i];
//      direction.Normalize();

//      float pitch = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
//      float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

//      if (pitch != 0 || yaw != 0)
//      {
//        print(pitch);

//        if(i == 0)
//        {
//          pitch = Mathf.Clamp(pitch, minPitchClampMcp, maxPitchClampMcp); // Example: MCP pitch range
//          yaw = Mathf.Clamp(yaw, minYawClampMcp, maxYawClampMcp);
//        }

//        else
//        {
//          pitch = Mathf.Clamp(pitch, minPitchClamp, maxPitchClamp); // Example: MCP pitch range
//          yaw = Mathf.Clamp(yaw, minYawClamp, maxYawClamp);
//        }

//        print("clamped  " + pitch);

//        // Use Quaternion.Euler to create a new rotation based on Euler angles
//        Vector3 eulerRotation = new Vector3(pitch, yaw, 0);  // No roll rotation for now
//        Quaternion targetRotation = Quaternion.Euler(eulerRotation);

//        // Calculate cumulative parent rotation up to the base joint
//        Quaternion cumulativeParentRotation = Quaternion.identity;
//        Transform currentParent = fingerJoints[i].parent;

//        while (currentParent != null && currentParent != handParent)
//        {
//          cumulativeParentRotation *= currentParent.localRotation;
//          currentParent = currentParent.parent;
//        }

//        // Apply the new rotation to the target, compensating for parent rotations
//        fingerJoints[i].localRotation = Quaternion.Inverse(cumulativeParentRotation) * targetRotation;
//      }
//    }
//  }
//}
//












//using UnityEngine;

//public class FingerRotation : MonoBehaviour
//{
//  // Hand model joints added in the Inspector for each finger
//  public Transform[] indexJoints;  // Assign in Inspector: MCP, PIP, DIP, Tip for Index finger
//  public Transform[] middleJoints; // Assign in Inspector: MCP, PIP, DIP, Tip for Middle finger
//  public Transform[] ringJoints;   // Assign in Inspector: MCP, PIP, DIP, Tip for Ring finger
//  public Transform[] pinkyJoints;  // Assign in Inspector: MCP, PIP, DIP, Tip for Pinky finger
//  public Transform[] thumbJoints;  // Assign in Inspector: MCP, PIP, DIP, Tip for Thumb

//  public Transform[] testIndexJoint; // Test joints for hand tracking (for testing)

//  public Transform handParent;

//  public float minPitchClampMcp;
//  public float maxPitchClampMcp;

//  public float minYawClampMcp;
//  public float maxYawClampMcp;

//  public float minPitchClamp;
//  public float maxPitchClamp;

//  public float minYawClamp;
//  public float maxYawClamp;

//  void Update()
//  {
//    // Update rotations for all fingers based on hand tracking data
//    UpdateFingerRotation(indexJoints);   // Index finger
//                                         //UpdateFingerRotation(middleJoints);  // Middle finger
//                                         //UpdateFingerRotation(ringJoints);    // Ring finger
//                                         //UpdateFingerRotation(pinkyJoints);   // Pinky finger
//                                         //UpdateFingerRotation(thumbJoints);   // Thumb
//  }

//  // Function to update finger rotation based on hand tracking data
//  void UpdateFingerRotation(Transform[] fingerJoints)
//  {
//    for (int i = 0; i < fingerJoints.Length - 1; i++)
//    {
//      // Retrieve hand tracking positions for the given finger (MCP, PIP, DIP, Tip)
//      Vector3[] handTrackingPositions = new Vector3[4];
//      handTrackingPositions[0] = testIndexJoint[0].position;  // MCP
//      handTrackingPositions[1] = testIndexJoint[1].position;  // PIP
//      handTrackingPositions[2] = testIndexJoint[2].position;  // DIP
//      handTrackingPositions[3] = testIndexJoint[3].position;  // Tip

//      // Calculate direction between consecutive joints
//      Vector3 direction = handTrackingPositions[i + 1] - handTrackingPositions[i];
//      direction.Normalize();

//      // Calculate the world space rotation for the joint
//      Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

//      // Clamp the pitch and yaw values if needed
//      float pitch = -Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
//      float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

//      //if (i == 0)
//      //{
//      //  pitch = Mathf.Clamp(pitch, minPitchClampMcp, maxPitchClampMcp); // MCP pitch range
//      //  yaw = Mathf.Clamp(yaw, minYawClampMcp, maxYawClampMcp);
//      //}
//      //else
//      //{
//      //  pitch = Mathf.Clamp(pitch, minPitchClamp, maxPitchClamp);
//      //  yaw = Mathf.Clamp(yaw, minYawClamp, maxYawClamp);
//      //}

//      // Apply clamping to the world space rotation
//      Vector3 eulerRotation = new Vector3(pitch, yaw, 0);
//      targetRotation = Quaternion.Euler(eulerRotation);

//      // Apply the target rotation in world space
//      fingerJoints[i].rotation = targetRotation;  // Use world rotation instead of local
//    }
//  }
//}



//using UnityEngine;


//class PitchRotation : MonoBehaviour
//{

//  public Transform middleMcp;

//  public Transform wrist;

//  public Transform target;

//  public Transform thumbMcp;



//  void Update()
//  {
//    Vector3 currentMiddleDirection = (middleMcp.position - wrist.position);


//    // Restrict thumbMcp within initial distance
//    Vector3 currentThumbDirection = (thumbMcp.position - wrist.position);


//    // Calculate directions for roll and yaw as before
//    Vector3 middleDirection = currentMiddleDirection;
//    middleDirection.Normalize();

//    Vector3 thumbDirection = currentThumbDirection;
//    thumbDirection.Normalize();

//    // Calculate yaw angle
//    //float yaw = Mathf.Atan2(middleDirection.x, middleDirection.z) * Mathf.Rad2Deg;

//    //// Calculate roll angle
//    //float roll = -Mathf.Atan2(thumbDirection.x, thumbDirection.y) * Mathf.Rad2Deg - 90;

//    // Calculate pitch angle
//    //float pitch = -Mathf.Atan2(middleDirection.y, middleDirection.z) * Mathf.Rad2Deg;

//    // Get the default forward direction of the wrist (e.g., the Z-axis in local space)
//    Vector3 wristForward = wrist.right;
//    // Compute the rotation required to go from wrist forward direction to the middle MCP direction
//    Quaternion rotation = Quaternion.FromToRotation(wristForward, middleDirection);

//    // Extract the pitch from the rotation
//    float pitch = rotation.eulerAngles.x;
//    float roll = rotation.eulerAngles.z;
//    float yaw = rotation.eulerAngles.y;

//    // Create rotations for pitch, yaw, and roll
//    Quaternion pitchRotation = Quaternion.AngleAxis(pitch, Vector3.right);
//    Quaternion yawRotation = Quaternion.AngleAxis(yaw, Vector3.up);
//    Quaternion rollRotation = Quaternion.AngleAxis(roll, Vector3.forward);

//    // Combine rotations: apply yaw, then pitch, then roll
//    Quaternion targetRotation = rollRotation;

//    //Quaternion targetRotation = pitchRotation;

//    // Apply the new rotation to the target
//    target.rotation = targetRotation;

//    print(pitch);
//  }
//}


using UnityEngine;

class CombinedRotation : MonoBehaviour
{
  public Transform middleMcp;
  public Transform wrist;
  public Transform target;
  public Transform thumbMcp;

  public float pitchOffset = 0;  // Optional pitch offset
  public float yawOffset = 0;    // Optional yaw offset
  public float rollOffset = -90; // Roll offset (adjust as needed)

  void Update()
  {
    // Calculate the rotation between wrist and middle MCP
    Quaternion rotation = Quaternion.LookRotation(middleMcp.position - wrist.position, thumbMcp.position - wrist.position);

    // Convert the rotation to Euler angles
    Vector3 eulerAngles = rotation.eulerAngles;

    // Extract the pitch, yaw, and roll
    float pitch = eulerAngles.x + pitchOffset;
    float yaw = eulerAngles.y + yawOffset;
    float roll = eulerAngles.z + rollOffset;

    // Apply the combined pitch, yaw, and roll to the target
    target.rotation = Quaternion.Euler(pitch, yaw, roll);

    // Debug print for all angles
    print($"Pitch: {pitch}, Yaw: {yaw}, Roll: {roll}");
  }
}


//using UnityEngine;

//class PitchRotation : MonoBehaviour
//{
//  public Transform middleMcp;
//  public Transform wrist;
//  public Transform target;
//  public Transform thumbMcp;

//  void Update()
//  {
//    // Get the local direction from wrist to middleMcp (local to the wrist)
//    Vector3 localMiddleDirection = wrist.InverseTransformDirection(middleMcp.position - wrist.position);

//    // Calculate pitch angle based on the local direction vector
//    float pitch = -Mathf.Atan2(localMiddleDirection.y, localMiddleDirection.z) * Mathf.Rad2Deg;

//    // Create rotations for pitch based on the local axis
//    Quaternion pitchRotation = Quaternion.AngleAxis(pitch, Vector3.right);

//    // Apply the new rotation to the target
//    target.rotation = pitchRotation;

//    print(pitch);
//  }
//}
