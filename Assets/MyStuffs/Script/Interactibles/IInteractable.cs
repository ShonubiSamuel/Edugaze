using UnityEngine;

public interface IInteractable
{
  void OnHover(Vector3 hitPoint);
  void OnHoverExit();
  void OnTouchEnter();
  void OnTouchExit();
  void OnGrab();
  void OnRelease();
  void OnPinch();
  void OnScale();
}
