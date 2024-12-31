using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown   _resolutionDropdown;
    [SerializeField] private Slider         _sensitivitySlider;
    [SerializeField] private MoveCamera     _cameraMovement;
    [SerializeField] private AudioMixer     audioMixer;

    private Resolution[] _resolutions;

    private void Start()
    {   
        _resolutions = Screen.resolutions
            .Where(resolution => Mathf.Approximately((float)resolution.width / resolution.height, 16f / 9f))
            .Select(resolution => new Resolution { width = resolution.width, height = resolution.height })
            .Distinct()
            .ToArray();

        _resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;

            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();


        _sensitivitySlider.value = _cameraMovement.GetSpeed();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSensitive(float sensitivity)
    {
        _cameraMovement.SetSpeed(sensitivity);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
