using UnityEngine;

public class SimulateParenting : MonoBehaviour
{
  public Transform parentObject; // Reference to the "parent" object
  public Transform[] childObjects;  // Array of references to the "child" objects

  private Vector3[] initialOffsets; // Array to store initial position offsets for each child
  private Quaternion[] initialRotationOffsets; // Array to store initial rotation offsets for each child

  void Start()
  {
    if (parentObject != null && childObjects.Length > 0)
    {
      // Initialize the offsets arrays based on the number of child objects
      initialOffsets = new Vector3[childObjects.Length];
      initialRotationOffsets = new Quaternion[childObjects.Length];

      // Calculate the initial offsets for each child object
      for (int i = 0; i < childObjects.Length; i++)
      {
        if (childObjects[i] != null)
        {
          // Calculate the initial position offset between the parent and child in the parent's local space
          initialOffsets[i] = parentObject.InverseTransformPoint(childObjects[i].position);

          // Calculate the initial rotation offset between the parent and child
          //initialRotationOffsets[i] = Quaternion.Inverse(parentObject.rotation) * childObjects[i].rotation;
        }
      }
    }
  }

  void Update()
  {
    if (parentObject != null && childObjects.Length > 0)
    {
      // Update each child object's position and rotation based on the parent's transform and initial offsets
      for (int i = 0; i < childObjects.Length; i++)
      {
        if (childObjects[i] != null)
        {
          // Update the child's position based on the parent's position and initial offset
          childObjects[i].position = parentObject.TransformPoint(initialOffsets[i]);

          // Update the child's rotation based on the parent's rotation and initial offset
          childObjects[i].rotation = parentObject.rotation * initialRotationOffsets[i];
        }
      }
    }
  }
}
