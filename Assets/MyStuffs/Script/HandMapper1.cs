using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandMapper1 : MonoBehaviour
{
  public List<Transform> firstHandTransforms = new List<Transform>();

  public Transform tip;
  public Transform dis;

  public Transform tipPositionController;
  public Transform disPositionController;

  bool isClicked;
  public void StartCalibration()
  {
    isClicked = true;
  }

  private void Update()
  {
    if (isClicked)
    {
      tip.position = tipPositionController.position;
      dis.position = disPositionController.position;
    }
    
    
  }
}
