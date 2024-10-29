using System.Collections.Generic;
using UnityEngine;

public class InteractableKeyboard : MonoBehaviour, IInteractable
{
  // Visual settings
  private Color originalColor; // Original button color
  public GameObject key; // Reference to the key GameObject
  private Renderer keyRenderer;

  // Audio settings
  public AudioClip touchEnterSound; // Sound to play on touch enter
  public AudioClip touchExitSound;  // Sound to play on touch exit
  private AudioSource audioSource;

  // Key press settings
  public float downOffset = 0.04f; // Offset when pressed
  private Vector3 initialPos;
  private bool isPressed;

  // Finger detection settings
  public float confirmExitTime = 0.5f; // Time to confirm finger has left
  private bool isFingerInside;
  private float exitTime;

  // Emission settings
  public Renderer targetRenderer; // Assign the renderer of the target GameObject in the Inspector
  public Color OnTouchColor;
  public float onClickEmissionIntensity = 9f;
  public float hoverColorIntensity = 1.5f;

  void Start()
  {
    // Initialize components
    keyRenderer = key.GetComponent<Renderer>();
    audioSource = GetComponent<AudioSource>();
    originalColor = keyRenderer.material.color;
    initialPos = key.transform.localPosition;

    // Initialize finger detection
    isFingerInside = false;
    exitTime = 0f;
  }

  void Update()
  {
    if (!isFingerInside && isPressed && Time.time - exitTime >= confirmExitTime)
    {
      OnTouchExit();
    }
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
    // Play touch enter sound and update key appearance
    MoveKeyDown();
    PlaySound(touchEnterSound);
    keyRenderer.material.color = OnTouchColor * onClickEmissionIntensity; // Color when pressed
    isPressed = true;

  }

  public void OnTouchExit()
  {
    // Play touch exit sound and reset key appearance
    PlaySound(touchExitSound);
    keyRenderer.material.color = originalColor;
    MoveKeyUp();
    isPressed = false;
    
  }

  public void OnGrab() { } // Not applicable for keyboard key

  public void OnRelease() { } // Not applicable for keyboard key

  public void OnPinch() { } // Not applicable for keyboard key

  public void OnScale() { } // Not applicable for keyboard key

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("fingers"))
    {

      IInteractable interactable = gameObject.GetComponent<IInteractable>();
      if (HandInteractionManager.Instance != null && interactable != null)
      {
        HandInteractionManager.Instance.currentInteractable = interactable;
      }
        isFingerInside = true;

      OnTouchEnter(); // Call OnTouchEnter when the finger enters
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

  private void PlaySound(AudioClip clip)
  {
    if (clip != null && audioSource != null)
    {
      audioSource.PlayOneShot(clip);
    }
  }

  private void MoveKeyDown()
  {
    Vector3 moveDirection = key.transform.TransformDirection(Vector3.down);
    key.transform.position += moveDirection * downOffset;
  }

  private void MoveKeyUp()
  {
    Vector3 moveDirection = key.transform.TransformDirection(Vector3.down);
    key.transform.position -= moveDirection * downOffset; // Move button back up
  }

}
