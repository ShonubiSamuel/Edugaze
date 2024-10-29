using UnityEngine;

public class HandInteractionManager : MonoBehaviour
{
  public LayerMask interactableLayer; // Layer for interactable objects
  private RaycastHit hitInfo;
  public IInteractable currentInteractable;
  private IInteractable interactable;


  public static HandInteractionManager Instance;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
  }

  void Update()
  {
    //DetectInteractions();
    HandInteraction();
  }

  void DetectInteractions()
  {
    // print(HandGestureRecognition.Instance.currentGestureFirst);
    if (HandGestureRecognition.Instance.currentGestureFirst == GestureType.Point)
    {
      Vector3 pointingPosition = HandLandmarksManager.Instance.firstHandLandmarks["index3"].position; // Adjust as needed
      Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(pointingPosition));

      // Draw the ray in the Scene view for visualization
      //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red); // Length of 10 units for visualization
      //Debug.Log("Ray Origin: " + ray.origin + " Direction: " + ray.direction);

      if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, interactableLayer))
      {
        if (hitInfo.collider != null)
        {
          interactable = hitInfo.collider.GetComponent<IInteractable>();

          if (interactable != null && interactable != currentInteractable)
          {
            if (currentInteractable != null)
            {
              //currentInteractable.OnHoverExit();
            }

            //currentInteractable = interactable;
            //currentInteractable.OnHover(hitInfo.point); // Pass the hit point
          }
        }
      }
      else
      {
        if (currentInteractable != null)
        {
          currentInteractable.OnHoverExit();

          if (HandGestureRecognition.Instance.currentGestureFirst == GestureType.OpenPalm || !HandLandmarksManager.Instance.firstHandParent.activeInHierarchy)
          {
            if (currentInteractable != null)
            {
              currentInteractable.OnHoverExit();
            }
            currentInteractable = null;
          }
        }
      }
    }
  }

  void HandInteraction()
  {

    if (currentInteractable != null)
    {

      //if (HandLandmarksManager.Instance.IsPinching())
      //{
      //  currentInteractable.OnPinch();

      //  if (!HandLandmarksManager.Instance.firstHandParent.activeInHierarchy)
      //  {
      //    if (currentInteractable != null)
      //    {
      //     // currentInteractable.OnHoverExit();
      //    }
      //    currentInteractable = null;
      //  }
      //}

      if (HandGestureRecognition.Instance.currentGestureFirst == GestureType.Grab)
      {
        currentInteractable.OnGrab();
      }

      if (!HandLandmarksManager.Instance.firstHandParent.activeInHierarchy)
      {
        if (currentInteractable != null)
        {
          currentInteractable.OnRelease();
        }
        currentInteractable = null;
      }
    }
  }
}
