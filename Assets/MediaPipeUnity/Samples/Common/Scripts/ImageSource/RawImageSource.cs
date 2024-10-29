using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
  public class RawImageSource : ImageSource
  {
    private const string _TAG = nameof(RawImageSource);

    [SerializeField]
    private ResolutionStruct[] _defaultAvailableResolutions = new ResolutionStruct[] {

        new ResolutionStruct(UnityEngine.Screen.width, UnityEngine.Screen.height, 60),
    };

    public override int textureWidth => _renderTexture != null ? _renderTexture.width : 0;
    public override int textureHeight => _renderTexture != null ? _renderTexture.height : 0;

    public override bool isVerticallyFlipped => false;
    public override bool isFrontFacing => false;
    public override RotationAngle rotation => !isPrepared ? RotationAngle.Rotation0 : 0;

    public override string sourceName => "AR Camera";
    public override string[] sourceCandidateNames => new string[] { "AR Camera" };

    public override ResolutionStruct[] availableResolutions => _defaultAvailableResolutions;

    public override bool isPrepared => _renderTexture != null;
    public override bool isPlaying => _renderTexture != null;

    private RenderTexture _renderTexture;
    [SerializeField]
    private ARCameraBackground _arCameraBackground;

    public RawImage rawImage;

    public override void SelectSource(int sourceId) { }

    public override IEnumerator Play()
    {
      yield return WaitForArCameraBackgroundMaterial();
      resolution = GetDefaultResolution();
    }


    public override IEnumerator Resume()
    {
      yield return new WaitUntil(() => _renderTexture != null);
    }

    public override void Pause() { }

    public override void Stop() { }

    public override Texture GetCurrentTexture()
    {
      CopyArCameraBackground();
      return _renderTexture;
    }

    private IEnumerator WaitForArCameraBackgroundMaterial()
    {
      // Wait until _arCameraBackground is not null
      yield return new WaitUntil(() => _arCameraBackground == null);

      _arCameraBackground = UnityEngine.Object.FindObjectOfType<ARCameraBackground>();
      Debug.Log("Waiting for ARCameraBackground material to be set...");
      yield return null; // Wait until the next frame

      // Once the material is available, proceed with the CopyArCameraBackground method
      CopyArCameraBackground();
    }

    public int doownscale = 1;

    private void CopyArCameraBackground()
    {
      // Create RenderTexture if it does not exist or if screen size has changed
      if (_renderTexture == null || _renderTexture.width != UnityEngine.Screen.width || _renderTexture.height != UnityEngine.Screen.height)
      {
        if (_renderTexture != null)
        {
          _renderTexture.Release();
        }
        _renderTexture = new RenderTexture(UnityEngine.Screen.width/ doownscale, UnityEngine.Screen.height/ doownscale, 24);
      }

      // Blit AR camera background to the RenderTexture
      if (_arCameraBackground == null || _arCameraBackground.material == null)
      {
        Debug.LogError("---- ARCameraBackground or its material is not set.");
        return;
      }
      Graphics.Blit(null, _renderTexture, _arCameraBackground.material);
    }


    private ResolutionStruct GetDefaultResolution()
    {
      return new ResolutionStruct(_renderTexture.width, _renderTexture.height, 60);
    }
  }
}
