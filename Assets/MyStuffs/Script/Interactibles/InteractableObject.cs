using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
  private MeshRenderer objectRenderer;
  private Material targetMaterial;

  public GameObject objectParent;
  public GameObject objectMainRenderer;

  public Color OnTouchColor;
  private Color originalColor;

  public float onClickEmissionIntensity = 9f;

  void Start()
  {
    objectRenderer = objectMainRenderer.GetComponent<MeshRenderer>();
    if (objectRenderer != null)
    {
      targetMaterial = objectRenderer.material;
      originalColor = targetMaterial.color;
    }
  }

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
    if (targetMaterial != null)
    {
      targetMaterial.color = originalColor * onClickEmissionIntensity;
    }
  }

  public void OnTouchExit()
  {
    if (targetMaterial != null)
    {
      targetMaterial.color = originalColor;
    }
  }

  public void OnGrab()
  {
    //MoveInteractibleObject.Instance.MoveObject(objectParent);
  }

  public void OnRelease()
  {
    OnTouchExit();
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

        OnTouchEnter();
      }

      
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("fingers"))
    {
       // Call OnTouchExit when the finger exits
    }
  }

 
}
