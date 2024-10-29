using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandMapper : MonoBehaviour
{
  public List<Transform> firstHandTransforms = new List<Transform>();


  public Transform wrist;
  public Transform modelwrist;

  bool isClicked;

  public void EnableRigBuilder()
  {
    isClicked = true;
  }

  public float offset;


  private void Update()
  {
    if (isClicked)
    {
      modelwrist.position = wrist.position;
    }
    else
    {
      int i = 0;
      foreach (var key in HandLandmarksManager.Instance.firstHandLandmarks)
      {
        firstHandTransforms[i].position = key.Value.position * offset;
        i++;
      }
    }
    
  }
}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Animations.Rigging;

//public class HandMapper : MonoBehaviour
//{
//  public List<Transform> firstHandTransforms = new List<Transform>();


//  public Transform wrist;
//  public Transform modelwrist;

//  public Transform RotationHandIndexTip;
//  public Transform RotationHandMiddleTip;
//  public Transform RotationHandRingTip;
//  public Transform RotationHandPinkyTip;

//  bool isClicked;

//  public void EnableRigBuilder()
//  {
//    isClicked = true;
//  }



//  private void Update()
//  {
//    if (isClicked)
//    {
//      modelwrist.position = wrist.position;
//    }
//    else
//    {
//      int i = 0;
//      foreach (var landmark in HandLandmarksManager.Instance.firstHandLandmarks)
//      {
//        if (landmark.Key == "index3")
//        {
//          firstHandTransforms[i].position = RotationHandIndexTip.position;
//        }
//        else if (landmark.Key == "middle3")
//        {
//          firstHandTransforms[i].position = RotationHandMiddleTip.position;
//        }
//        else if (landmark.Key == "ring3")
//        {
//          firstHandTransforms[i].position = RotationHandRingTip.position;
//        }
//        else if (landmark.Key == "pinky3")
//        {
//          firstHandTransforms[i].position = RotationHandPinkyTip.position;
//        }
//        else
//        {
//          firstHandTransforms[i].position = landmark.Value.position;
//        }
//        i++;

//      }
//    }
//  }
//}
