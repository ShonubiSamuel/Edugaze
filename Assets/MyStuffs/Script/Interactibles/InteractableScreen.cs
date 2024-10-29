using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScreen : MonoBehaviour
{


  public GameObject objectParent;

  public float onClickEmissionIntensity = 9f;
  public float hoverColorIntensity = 1.5f;


  public void OnHover(Vector3 hitPoint)
  {
    // EnableEmission();
  }

  public void OnHoverExit()
  {
    // DisableEmission();
  }

  public void OnTouchEnter()
  {
    
  }

  public void OnTouchExit()
  {
    
  }

  public void OnGrab()
  {
    //MoveInteractibleObject.Instance.MoveObject(objectParent);
  }

  public void OnRelease()
  {
    // Perform release action
  }

  public void OnPinch()
  {
    //SmoothRotateByMovementSpeed.Instance.RotateObject(objectParent);
  }

  public void OnScale()
  {
    // Perform scaling action
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("fingers"))
    {
      IInteractable interactable = gameObject.GetComponent<IInteractable>();
      if (HandInteractionManager.Instance != null && interactable != null)
      {
        HandInteractionManager.Instance.currentInteractable = interactable;

        //MoveInteractibleObject.Instance.isFirstFrame = true;
        //SmoothRotateByMovementSpeed.Instance.isFirstFrame = true;

        //if(HandGestureRecognition.Instance.currentGestureFirst != GestureType.Grab || HandGestureRecognition.Instance.currentGestureFirst != GestureType.Pinch)
        //{
        //  OnTouchEnter();
        //}
        OnTouchEnter();
        // Call OnTouchEnter when the finger enters
      }


    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("fingers"))
    {
      OnTouchExit(); // Call OnTouchExit when the finger exits
    }
  }

  public void EnableEmission()
  {
    
  }

  public void DisableEmission()
  {
    
  }
}
