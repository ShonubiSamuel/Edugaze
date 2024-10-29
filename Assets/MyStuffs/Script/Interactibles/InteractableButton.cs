using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : MonoBehaviour, IInteractable
{
  // Visual settings
  private Color originalColor; // Original button color
  public GameObject button; // Reference to the button GameObject
  private Renderer buttonRenderer;

  // Audio settings
  public AudioClip touchEnterSound; // Sound to play on touch enter
  public AudioClip touchExitSound;  // Sound to play on touch exit
  private AudioSource audioSource;

  // Button press settings
  public float downOffset = 0.03f; // Offset when pressed
  private Vector3 initialPos;
  private bool isPressed;

  // Finger detection settings
  private bool isFingerInside;
  public float confirmExitTime = 0.5f; // Time to confirm finger has left
  private float exitTime;

  // Emission settings
  public Renderer targetRenderer; // Assign the renderer of the target GameObject in the Inspector
  private Material targetMaterial;

  public Color OnTouchColor;
  public float onClickEmissionIntensity = 9f;
  public float hoverColorIntensity = 1.5f;

  void Start()
  {
    // Initialize components
    buttonRenderer = button.GetComponent<Renderer>();
    audioSource = GetComponent<AudioSource>();
    originalColor = buttonRenderer.material.color;
    initialPos = button.transform.localPosition;
    isFingerInside = false;
    exitTime = 0f;

    // Get the material from the renderer
    targetMaterial = targetRenderer.material;
  }

  public void OnHover(Vector3 hitPoint)
  {
    // Enable emission
  }

  public void OnHoverExit()
  {
    // Disable emission
  }

  public void OnTouchEnter()
  {

    IInteractable interactable = gameObject.GetComponent<IInteractable>();
    if (HandInteractionManager.Instance != null && interactable != null)
    {
      HandInteractionManager.Instance.currentInteractable = interactable;
    }

    // Play touch enter sound and update button appearance
    PlaySound(touchEnterSound);
    buttonRenderer.material.color = OnTouchColor * onClickEmissionIntensity; // Color when pressed
    Vector3 moveDirection = button.transform.TransformDirection(Vector3.down);
    button.transform.position += moveDirection * downOffset;
    isPressed = true;
    
  }

  public void OnTouchExit()
  {
    // Play touch exit sound and reset button appearance
    PlaySound(touchExitSound);
    buttonRenderer.material.color = originalColor;
    Vector3 moveDirection = button.transform.TransformDirection(Vector3.down);
    button.transform.position -= moveDirection * downOffset; // Move button back up
    isPressed = false;
    
  }

  public void OnGrab() { } // Not applicable for button

  public void OnRelease() { } // Not applicable for button

  public void OnPinch() { } // Not applicable for button

  public void OnScale() { } // Not applicable for button

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("fingers"))
    {
      isFingerInside = true;

      if (!isPressed)
      {
        OnTouchEnter(); // Call OnTouchEnter when the finger enters
      }
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("fingers"))
    {
      isFingerInside = false;
      exitTime = Time.time;
    }
  }

  void Update()
  {
    if (!isFingerInside && isPressed && Time.time - exitTime >= confirmExitTime)
    {
      OnTouchExit(); // Call OnTouchExit when the finger exits
    }
  }


  private void PlaySound(AudioClip clip)
  {
    if (clip != null && audioSource != null)
    {
      audioSource.PlayOneShot(clip);
    }
  }
}
