using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.Json; //WitResponseNode
using Meta.WitAi;  //GetIntents();
using TMPro;


public class ResponseAnalyzer : MonoBehaviour
{

  public delegate void AttackNotify();
  public event AttackNotify ProcessCompleted;

  public TextMeshProUGUI intent;
  public static ResponseAnalyzer instance;

  public int familiarSpawnIndex;

  //CharacterInputHandler characterInputHandler;

  public TextMeshProUGUI transText;

  public void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(this.gameObject);
      return;
    }
    Destroy(this.gameObject);
  }

  string follow = "follow";
  string attack = "attack";
  string closeStatus = "closeStatus";
  string openStatus = "openStatus";
  string summon = "summon";

  [HideInInspector]
  public string familiarToSummon;

  private void OnEnable()
  {
    VoiceCommands.OnResponseEvent += HandleResponseEvent;
  }

  private void OnDisable()
  {
    VoiceCommands.OnResponseEvent -= HandleResponseEvent;
  }

  private void HandleResponseEvent(WitResponseNode response)
  {
    intent.text = response.GetIntentName();
    if (response.GetIntentName() == follow)
    {
      print("intent is follow");
      GetAttackName(response);
    }
    if (response.GetIntentName() == attack)
    {
      print("intent is attack");
      GetAttackName(response);
    }
    if (response.GetIntentName() == closeStatus)
    {
      //ActivateStatusWindow(false);
      print("intent is close status");
    }
    if (response.GetIntentName() == openStatus)
    {
      transText.text = "Open Status";
      StartCoroutine(clearTranscription());
      //ActivateStatusWindow(true);
      ////print("intent is open status");
    }
    if (response.GetIntentName() == summon)
    {
      
      GetSummonName(response);
    }

  }


  void GetAttackName(WitResponseNode response)
  {
    print("get attacc name");
    print("---- " + response.GetIntentName());

    string intentName = response.GetIntentName();


    if(intentName == "follow")
    {
      print("---- it's working");
    }

    //print("Get trans - " + response.GetTranscription());
    //string[] attackNames = response.GetAllEntityValues("atackName:attackName");



    //foreach(var attackName in attackNames)
    //{
    //  print(attackName);
    //  if(attackName.ToLower() == "magic beam")
    //  {
    //    transText.text = "Magic beam";
    //    StartCoroutine(clearTranscription());
    //    ProcessCompleted?.Invoke();
    //  }
    //}
  }

  IEnumerator clearTranscription()
  {
    yield return new WaitForSeconds(5f);
    transText.text = "";
  }

  void GetSummonName(WitResponseNode response)
  {

    //if (characterInputHandler == null && NetworkPlayer.Local != null)
    //  characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();


    //string[] familiarsToSummon = response.GetAllEntityValues("playerName:playerName");


    //foreach (var familiarNaame in familiarsToSummon)
    //{
    //  List<Familiars> listOfFamiliars = ListOfFamiliars.instance.myList;
    //  for (int i = 0; i < listOfFamiliars.Count; i++)
    //  {
    //    bool containsName = listOfFamiliars[i].name == familiarNaame;

    //    if (containsName)
    //    {
    //      if (characterInputHandler != null)
    //      {
    //        characterInputHandler.VoiceIMageName(familiarNaame);
    //      }

    //      return;
    //    }
    //    else
    //    {
    //      print("ResponseAnalyzer - no  contains");
    //    }

    //  }


   // }

  }
//public GameObject StatusWindow;
//public float timef = 5f;
//  public void ActivateStatusWindow(bool active){

//    StatusWindow.SetActive(active);

//    if(active){

//      StartCoroutine(wait(timef));
      
//    }
//  }

//  IEnumerator wait(float time){

//    yield return new WaitForSeconds(time);
//    StatusWindow.SetActive(false);

    
//  }


}
