using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoplayerManagering : MonoBehaviour
{
  public VideoPlayer videoplayer;

  public void Play()
  {
    videoplayer.Play();
  }

  public void Pause()
  {
    videoplayer.Pause();
  }
}
