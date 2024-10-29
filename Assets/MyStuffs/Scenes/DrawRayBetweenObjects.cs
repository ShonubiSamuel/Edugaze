using UnityEngine;

public class DrawRayBetweenObjects : MonoBehaviour
{
  public GameObject objectA; // First object
  public GameObject objectB; // Second object
  public Color rayColor = Color.red; // Ray color

  void Update()
  {
    if (objectA != null && objectB != null)
    {
      // Get direction from objectA to objectB
      Vector3 direction = objectB.transform.position - objectA.transform.position;

      // Draw a ray between objectA and objectB
      Debug.DrawRay(objectA.transform.position, direction, rayColor);
    }
  }
}
