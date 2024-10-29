using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSlider : MonoBehaviour, IInteractable
{
  public void OnHover(Vector3 hitPoint)
  {
    // Highlight slider
  }

  public void OnTouchEnter()
  {
    // Perform button click action
  }

  public void OnTouchExit()
  {
    // Perform button click action
  }

  public void OnGrab()
  {
    // Perform grab action
  }

  public void OnRelease()
  {
    // Perform release action
  }

  public void OnPinch()
  {
    // Perform slider movement action
  }

  public void OnHoverExit() { }

  public void OnScale() { } // Not applicable for slider
}