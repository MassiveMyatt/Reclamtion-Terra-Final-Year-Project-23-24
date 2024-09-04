using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuManager : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public TMP_Dropdown qualityDropDown;
    public Toggle screenButton;

    /// <summary>
    /// This method sets the games graphics quality based on the user's setting choice
    /// </summary>
    /// <param name="Index">The index of the quality level</param>
    public void SetQuality (int Index)
    {

        QualitySettings.SetQualityLevel (Index);

    }
    /// <summary>
    /// This method sets the game to either fullscreen or not depeding on the user
    /// </summary>
    /// <param name="fullscreen">bool of if the user wants fullscreen enabled or disabled.</param>
    public void SetFullscreen (bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
    /// <summary>
    /// This method sets the volume of the game given the user input
    /// </summary>
    /// <param name="slideVolume">The user's volume input</param>
    public void SetVolume (float slideVolume)
    {
        audioMixer.SetFloat("volume", slideVolume);
        Debug.Log(slideVolume);
    }
    /// <summary>
    /// Method to set the playerpref for volume.
    /// </summary>
    public void VolumePrefSet()
    {
        float playerVolume = PlayerPrefs.GetFloat("volume", 1);
        volumeSlider.value = playerVolume;

        volumeSlider.onValueChanged.AddListener(delegate
        {
            PlayerPrefs.SetFloat("volume", volumeSlider.value);
        });
    }
    /// <summary>
    /// Method to set the playerpref for graphics quality.
    /// </summary>
    public void GraphicPrefSet()
    {
        int qualityIndex = PlayerPrefs.GetInt("quality", 1);
        qualityDropDown.value = qualityIndex;

        qualityDropDown.RefreshShownValue();

        qualityDropDown.onValueChanged.AddListener(delegate
        {
            PlayerPrefs.SetInt("quality", qualityDropDown.value);
        });
    }
    /// <summary>
    /// Method to set the playerpref for fullscreen.
    /// </summary>
    public void ScreenPrefSet()
    {
        bool isOn = PlayerPrefs.GetInt("fullscreen", 0) == 1;
        screenButton.isOn = isOn;
        screenButton.onValueChanged.AddListener(delegate
        {
            PlayerPrefs.SetInt("fullscreen", isOn ? 1 : 0);
        });
    }

    /// <summary>
    /// Method is called when the scene loads. Setting each setting option to the players prefrence from the 
    /// last time they loaded this scene.
    /// </summary>
    private void Start()
    {
        VolumePrefSet();
        GraphicPrefSet();
        ScreenPrefSet();
    }
}
