using UnityEngine;
using System.IO;
using TMPro;

public class HandLandmarkAnalysis : MonoBehaviour
{
  private string filePath;
  private StreamWriter writer;
  private bool isReady;
  public TextMeshProUGUI buttonText;

  void Start()
  {
    // Use the custom path for the CSV file
    filePath = Path.Combine(Application.persistentDataPath, "hand_landmark_analysis.csv");

    // Create the directory if it doesn't exist
    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
  }

  public void ToggleReady()
  {
    if (isReady)
    {
      // Stop recording and share data
      buttonText.text = "stopped";
      isReady = false;
      if (writer != null)
      {
        writer.Close();
        writer = null;
        Debug.Log($"Data written to {filePath}");
      }
      ShareData();
    }
    else
    {
      // Start recording
      buttonText.text = "recording";
      isReady = true;
      if (writer == null)
      {
        writer = new StreamWriter(filePath);
        writer.WriteLine("IndexMCPToWrist,MiddleMCPToWrist,RingMCPToWrist,PinkyMCPToWrist,IndexPIPToMCP,MiddlePIPToMCP,RingPIPToMCP,PinkyPIPToMCP,IndexDIPToPIP,MiddleDIPToPIP,RingDIPToPIP,PinkyDIPToPIP,IndexTipToDIP,MiddleTipToDIP,RingTipToDIP,PinkyTipToDIP,IndexToMiddleMCP,MiddleToRingMCP,RingToPinkyMCP");
      }
    }
  }

  void Update()
  {
    if (isReady)
    {
      AnalyseHandLandmarks();
    }
  }

  void OnDestroy()
  {
    // Ensure the StreamWriter is closed if the script is destroyed
    if (writer != null)
    {
      writer.Close();
      Debug.Log($"Data written to {filePath}");
    }
  }

  void AnalyseHandLandmarks()
  {
    var handManager = HandLandmarksManager.Instance;

    if (handManager == null || handManager.firstHandLandmarks.Count == 0)
    {
      Debug.LogError("HandLandmarksManager instance or landmarks are not available.");
      return;
    }

    // Retrieve the relevant landmarks
    Transform wrist = handManager.firstHandLandmarks["wrist"];
    Transform indexMCP = handManager.firstHandLandmarks["index0"];
    Transform indexPIP = handManager.firstHandLandmarks["index1"];
    Transform indexDIP = handManager.firstHandLandmarks["index2"];
    Transform indexTip = handManager.firstHandLandmarks["index3"];

    Transform middleMCP = handManager.firstHandLandmarks["middle0"];
    Transform middlePIP = handManager.firstHandLandmarks["middle1"];
    Transform middleDIP = handManager.firstHandLandmarks["middle2"];
    Transform middleTip = handManager.firstHandLandmarks["middle3"];

    Transform ringMCP = handManager.firstHandLandmarks["ring0"];
    Transform ringPIP = handManager.firstHandLandmarks["ring1"];
    Transform ringDIP = handManager.firstHandLandmarks["ring2"];
    Transform ringTip = handManager.firstHandLandmarks["ring3"];

    Transform pinkyMCP = handManager.firstHandLandmarks["pinky0"];
    Transform pinkyPIP = handManager.firstHandLandmarks["pinky1"];
    Transform pinkyDIP = handManager.firstHandLandmarks["pinky2"];
    Transform pinkyTip = handManager.firstHandLandmarks["pinky3"];

    // Calculate distances to the wrist (MCP to Wrist)
    float indexMCPToWrist = Vector3.Distance(indexMCP.position, wrist.position);
    float middleMCPToWrist = Vector3.Distance(middleMCP.position, wrist.position);
    float ringMCPToWrist = Vector3.Distance(ringMCP.position, wrist.position);
    float pinkyMCPToWrist = Vector3.Distance(pinkyMCP.position, wrist.position);

    // Calculate distances between MCP joints
    float indexToMiddleMCP = Vector3.Distance(indexMCP.position, middleMCP.position);
    float middleToRingMCP = Vector3.Distance(middleMCP.position, ringMCP.position);
    float ringToPinkyMCP = Vector3.Distance(ringMCP.position, pinkyMCP.position);

    // Calculate distances between each segment (PIP to MCP, DIP to PIP, Tip to DIP)
    float indexPIPToMCP = Vector3.Distance(indexPIP.position, indexMCP.position);
    float indexDIPToPIP = Vector3.Distance(indexDIP.position, indexPIP.position);
    float indexTipToDIP = Vector3.Distance(indexTip.position, indexDIP.position);

    float middlePIPToMCP = Vector3.Distance(middlePIP.position, middleMCP.position);
    float middleDIPToPIP = Vector3.Distance(middleDIP.position, middlePIP.position);
    float middleTipToDIP = Vector3.Distance(middleTip.position, middleDIP.position);

    float ringPIPToMCP = Vector3.Distance(ringPIP.position, ringMCP.position);
    float ringDIPToPIP = Vector3.Distance(ringDIP.position, ringPIP.position);
    float ringTipToDIP = Vector3.Distance(ringTip.position, ringDIP.position);

    float pinkyPIPToMCP = Vector3.Distance(pinkyPIP.position, pinkyMCP.position);
    float pinkyDIPToPIP = Vector3.Distance(pinkyDIP.position, pinkyPIP.position);
    float pinkyTipToDIP = Vector3.Distance(pinkyTip.position, pinkyDIP.position);

    // Write to CSV
    writer.WriteLine($"{indexMCPToWrist},{middleMCPToWrist},{ringMCPToWrist},{pinkyMCPToWrist},{indexPIPToMCP},{middlePIPToMCP},{ringPIPToMCP},{pinkyPIPToMCP},{indexDIPToPIP},{middleDIPToPIP},{ringDIPToPIP},{pinkyDIPToPIP},{indexTipToDIP},{middleTipToDIP},{ringTipToDIP},{pinkyTipToDIP},{indexToMiddleMCP},{middleToRingMCP},{ringToPinkyMCP}");
  }

  public void ShareData()
  {
    if (File.Exists(filePath))
    {
      new NativeShare()
          .AddFile(filePath)
          .SetSubject("Hand Calibration Data")
          .SetText("Here is the hand gesture calibration file.")
          .SetCallback((result, shareTarget) =>
          {
            Debug.Log("Share result: " + result + ", selected app: " + shareTarget);
          })
          .Share();
    }
    else
    {
      Debug.LogError("File does not exist: " + filePath);
    }
  }
}
