using UnityEngine;

public class HandScaler : MonoBehaviour
{
  public GameObject virtualHand; // The virtual hand model to be scaled

  // Real hand tracking data
  public Transform realThumbMCP; // Thumb MCP joint of the real hand
  public Transform realThumbTip; // Thumb tip of the real hand
  public Transform realPinkyMCP; // Pinky MCP joint of the real hand
  public Transform realPinkyTip; // Pinky tip of the real hand

  // Virtual hand model data
  public Transform virtualThumbMCP; // Thumb MCP joint of the virtual hand
  public Transform virtualThumbTip; // Thumb tip of the virtual hand
  public Transform virtualPinkyMCP; // Pinky MCP joint of the virtual hand
  public Transform virtualPinkyTip; // Pinky tip of the virtual hand

  private Vector3 initialScale; // Store the initial scale of the virtual hand

  public void HandScale()
  {
    // Store the initial scale of the virtual hand model
    initialScale = virtualHand.transform.localScale;
    UpdateScale();
  }

  void UpdateScale()
  {
    // Check that all the required joints are assigned
    if (realThumbMCP != null && realThumbTip != null && realPinkyMCP != null && realPinkyTip != null &&
        virtualThumbMCP != null && virtualThumbTip != null && virtualPinkyMCP != null && virtualPinkyTip != null)
    {
      // Measure the real hand's thumb length (MCP to tip)
      float realThumbLength = Vector3.Distance(realThumbMCP.position, realThumbTip.position);

      // Measure the real hand's pinky length (MCP to tip)
      float realPinkyLength = Vector3.Distance(realPinkyMCP.position, realPinkyTip.position);

      // Calculate the real hand size by averaging the lengths of the thumb and pinky
      float realHandSize = (realThumbLength + realPinkyLength) / 2f;

      // Measure the virtual hand's thumb length (MCP to tip)
      float virtualThumbLength = Vector3.Distance(virtualThumbMCP.position, virtualThumbTip.position);

      // Measure the virtual hand's pinky length (MCP to tip)
      float virtualPinkyLength = Vector3.Distance(virtualPinkyMCP.position, virtualPinkyTip.position);

      // Calculate the virtual hand size by averaging the lengths of the thumb and pinky
      float virtualHandSize = (virtualThumbLength + virtualPinkyLength) / 2f;

      // Calculate the scaling factor based on the ratio of real to virtual hand size
      float scaleMultiplier = realHandSize / virtualHandSize;

      // Apply the scaling factor to the virtual hand model, maintaining proportions
      virtualHand.transform.localScale = initialScale * scaleMultiplier;
    }
  }
}
