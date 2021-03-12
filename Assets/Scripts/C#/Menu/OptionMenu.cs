using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <author>Christian</author>
/// <summary>
/// Set Optionmenu values and react on Optionmenu events
/// </summary>
public class OptionMenu : MonoBehaviour
{
#pragma warning disable CS0649

    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown graphicsDropdown;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider[] volumeSlider;
    [SerializeField] List<string> graphicList;

    Resolution[] resolutions;
#pragma warning restore CS0649

    #region Start Methods
    private void Start()
    {
        if(resolutionDropdown && graphicsDropdown)
        {
            GetResolution();
            GetGraphic();
            GetFullscreen();

            InstantiateVolume();
        }
    }

    /// <summary>
    /// Put aviable Screen Resolutions into the dropdown
    /// </summary>
    private void GetResolution()
    {

        List<string> options = new List<string>();
        int? currentResIndex = 0;

        resolutions = Screen.resolutions;

        resolutionDropdown?.ClearOptions();

        if (SaveManager.instance)
        {
            currentResIndex = SaveManager.instance.GetSettingsInt("resolution");

            if (currentResIndex == null)
            {
                currentResIndex = 0;
            }
        }

        // Iterate through every resolution and set resolutionDropdown
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].ToString().ToLower());

            // Set the resolutionDropdownindex to the current Resolution
            if (currentResIndex <= 0 && resolutions[i].ToString().Equals(Screen.currentResolution.ToString()))
            {
                currentResIndex = i;
            }
        }

        // Add List to Dropdown
        resolutionDropdown.AddOptions(options);
       

        resolutionDropdown.value = (int)currentResIndex;

        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener((int value) => SetResolution(value));
    }

    /// <summary>
    /// Put aviable Qualitylevel into Dropdown
    /// </summary>
    private void GetGraphic()
    {
        graphicsDropdown.ClearOptions();
        graphicsDropdown.AddOptions(graphicList);

        int? valueSave;

        // Get Saved Data
        if(SaveManager.instance && (valueSave = SaveManager.instance.GetSettingsInt("graphics")) != null)
        {
            graphicsDropdown.value = (int)valueSave;
            SetQuality(graphicsDropdown.value);
        }
        else
        {
            graphicsDropdown.value = QualitySettings.GetQualityLevel();
        }

        graphicsDropdown.RefreshShownValue();
        graphicsDropdown.onValueChanged.AddListener((int value) => SetQuality(value));
    }

    void GetFullscreen()
    {
        int? value;

        // Get Saved Data HERE
        if (SaveManager.instance && (value = SaveManager.instance.GetSettingsInt("fullscreen")) != null)
        {
            fullscreenToggle.isOn = (int)value >= 1;
        }
        else
        {
            fullscreenToggle.isOn = true;
        }

        fullscreenToggle.onValueChanged.AddListener((bool fullscreen) => SetFullscreen(fullscreen));
    }

    private void InstantiateVolume()
    {
        for (int i = 0; i < AudioManager.Instance.volumeStrings.Length; i++)
        {
            if(volumeSlider[i])
            {
                switch(i)
                {
                    case 0:
                        volumeSlider[i].onValueChanged.AddListener((float volume) => SetMasterVolume(volume));
                        break;
                    case 1:
                        volumeSlider[i].onValueChanged.AddListener((float volume) => SetMusicVolume(volume));
                        break;
                    case 2:
                        volumeSlider[i].onValueChanged.AddListener((float volume) => SetFXVolume(volume));
                        break;
                    case 3:
                        volumeSlider[i].onValueChanged.AddListener((float volume) => SetCharacterVolume(volume));
                        break;
                }
            }

            GetVolume(i);
        }
    }

    /// <summary>
    /// Put current Volume into slider
    /// </summary>
    private void GetVolume(int index)
    {
        float? volume;

        if(SaveManager.instance && (volume = SaveManager.instance.GetSettingsFloat(AudioManager.Instance.volumeStrings[index])) != null)
        {
            AudioManager.Instance.SetCurrentVolume(index, (float)volume);
        }
        else
        {
            volume = AudioManager.Instance.GetCurrentVolume(index);
        }

        volumeSlider[index].value = (float)volume;
    }

    #endregion

    #region SetEvent Methods

    /// <summary>
    /// Set the Volume on changed value
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        AudioManager.Instance.SetCurrentVolume(0, volume);
        SaveManager.instance?.SaveSettings(AudioManager.Instance.volumeStrings[0], volume);
    }


    /// <summary>
    /// Set the Volume on changed value
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetCurrentVolume(1, volume);
        SaveManager.instance?.SaveSettings(AudioManager.Instance.volumeStrings[1], volume);
    }

    /// <summary>
    /// Set the Volume on changed value
    /// </summary>
    public void SetFXVolume(float volume)
    {
        AudioManager.Instance.SetCurrentVolume(2, volume);
        SaveManager.instance?.SaveSettings(AudioManager.Instance.volumeStrings[2], volume);
    }

    /// <summary>
    /// Set the Volume on changed value
    /// </summary>
    public void SetCharacterVolume(float volume)
    {
        AudioManager.Instance.SetCurrentVolume(3, volume);
        SaveManager.instance?.SaveSettings(AudioManager.Instance.volumeStrings[3], volume);
    }

    /// <summary>
    /// Set graphicQuality on changed value
    /// </summary>
    public void SetQuality(int qualityIndex)
    {
        // Save Data HERE
        QualitySettings.SetQualityLevel(qualityIndex, true);
        SaveManager.instance?.SaveSettings("graphics", qualityIndex);
    }

    /// <summary>
    /// Set Fullscreen true/false
    /// </summary>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SaveManager.instance?.SaveSettings("fullscreen", isFullscreen ? 1 : 0);
    }

    /// <summary>
    /// Set resolution on changed value
    /// </summary>
    public void SetResolution(int resolutionIndex)
    {
        // Save Data HERE
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, fullscreenToggle.isOn);
        SaveManager.instance?.SaveSettings("resolution", resolutionIndex);
    }
    #endregion
}
