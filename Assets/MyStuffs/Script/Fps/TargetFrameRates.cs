using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrameRates : MonoBehaviour
{
  void Start()
  {
    QualitySettings.vSyncCount = 0;
    // Make the game run as fast as possible
    Application.targetFrameRate = -1;
    // Limit the framerate to 60
    Application.targetFrameRate = 60;
  }
}
