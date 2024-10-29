using System;
using System.Collections;
using UnityEngine;
using Oculus.Voice; //AppVoiceExperience
using Meta.WitAi.Json; //WitResponseNode
using Meta.WitAi;       //GetIntents();
//                        //void OnRequestResponse(WitResponseNode response)
//                        //{
////    WitIntentData[] colorNames = response.GetIntents();
//using Meta.WitAi.Data.Intents; //WitIntentData  {WitIntentData[] colorNames = response.GetIntents();}
//using Meta.WitAi.Data.Entities;
using TMPro;


public class VoiceCommands : MonoBehaviour
{
  public static VoiceCommands instance;

  public delegate void ResponseNodeEventHandler(WitResponseNode response);
  public static event ResponseNodeEventHandler OnResponseEvent;

  [SerializeField] AppVoiceExperience appVoiceExperience;
  //[SerializeField] private DictationService _dictation;

  public float waitTime;
  public TextMeshProUGUI onStartStopText;
  public TextMeshProUGUI transText;

  [HideInInspector]
  public string userResponse;


  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      return;
    }
    Destroy(this.gameObject);
  }

  private void OnEnable()
  {
    appVoiceExperience.VoiceEvents.OnResponse.AddListener(OnRequestResponse);
    appVoiceExperience.VoiceEvents.OnStartListening.AddListener(onStartListening);
    appVoiceExperience.VoiceEvents.OnStoppedListening.AddListener(onStoppedListening);
    appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(onPartialTranscription);
    appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(onFullTranscription);


    //_dictation.DictationEvents.OnStartListening.AddListener(onStartListening);
    //_dictation.DictationEvents.OnStoppedListening.AddListener(onStoppedListening);
    //_dictation.DictationEvents.OnPartialTranscription.AddListener(onPartialTranscription);
    //_dictation.DictationEvents.OnFullTranscription.AddListener(onFullTranscription);

    //StartCoroutine(WaitForPlayer());
  }

  private void OnDisable()
  {
    appVoiceExperience.VoiceEvents.OnResponse.RemoveListener(OnRequestResponse);
    appVoiceExperience.VoiceEvents.OnStartListening.RemoveListener(onStartListening);
    appVoiceExperience.VoiceEvents.OnStoppedListening.RemoveListener(onStoppedListening);
    appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(onPartialTranscription);
    appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(onFullTranscription);
  }



  void TextActivation(string text)
  {
    appVoiceExperience.Activate(text);
  }


  public float gestureHoldDuration = 2.0f; // Time in seconds to maintain the gesture
  private float gestureHoldTimer = 0.0f;
  private bool isGestureActive = false;

  private void Update()
  {
    // Check if the current gesture is a pointing gesture
    if (HandGestureRecognition.Instance.currentGestureFirst == GestureType.Point)
    {
      // If the gesture is active, start the timer
      if (!isGestureActive)
      {
        isGestureActive = true;
        gestureHoldTimer = 0.0f;
      }

      // Increment the timer if the gesture is still active
      gestureHoldTimer += Time.deltaTime;

      // Check if the gesture has been held for the required duration
      if (gestureHoldTimer >= gestureHoldDuration)
      {
        ReActivate();
        gestureHoldTimer = 0.0f; // Reset the timer
      }
    }
    else
    {
      // Reset the timer and state if the gesture is not active
      isGestureActive = false;
      gestureHoldTimer = 0.0f;
    }

    if (appVoiceExperience.Active)
    {
      if (HandGestureRecognition.Instance.currentGestureFirst != GestureType.Point)
      {
        appVoiceExperience.Deactivate();
      }
    }
  }


  public void Activate()
  {
    appVoiceExperience.Activate();
  }

  public void ReActivate()
  {
    if (appVoiceExperience.Active)
    {
      return;
    }

    else
    {
      Activate();
    }
  }

  public void Deactivate()
  {
    appVoiceExperience.Deactivate();
    SmoothFollowCameraWithRotationOffset.Instance.shouldFollow = false;
  }

  void onStartListening()
  {
    onStartStopText.text = "Listening";
  }

  void onStoppedListening()
  {
    onStartStopText.text = "Stopped Listening";


    if (string.IsNullOrEmpty(userResponse))
    {
      //ReActivate();
    }
  }

  void onPartialTranscription(string transcription)
  {
    transText.text = transcription;
    //print("partial  -- " + transcription);
    //Deactivate();
    //TextActivation(transcription);
  }

  void onFullTranscription(string transcription)
  {
    transText.text = transcription;
    //TextActivation(transcription);

    //print("full  -- " + transcription);
  }

  public void OnRequestResponse(WitResponseNode response)
  {
    print("--- name" + response.GetIntentName());

    if(response.GetIntentName() == "follow")
    {
      SmoothFollowCameraWithRotationOffset.Instance.shouldFollow = true;
    }
    if(response.GetIntentName() == "do_not_follow")
    {
      SmoothFollowCameraWithRotationOffset.Instance.shouldFollow = false;
    }
    //OnResponseEvent?.Invoke(response);
  }
}
