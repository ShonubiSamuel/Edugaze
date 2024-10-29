using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
  // Display UI
  [SerializeField]
  TextMeshProUGUI currentFPS;

  public float updateInterval = 0.5f;

  float timeleft;

  void Start()
  {
    //Application.targetFrameRate = 60;
  }

  void Update()
  {

    timeleft -= Time.deltaTime;
    if (timeleft <= 0.0)
    {
      timeleft = updateInterval;

      float fps = 1 / Time.unscaledDeltaTime;
      currentFPS.text = "Cur. FPS: " + (int)fps;

    }
  }
}
