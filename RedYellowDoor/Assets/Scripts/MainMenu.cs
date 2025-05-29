using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    [SerializeField] private GameObject langObject;

    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle subtitlesToggle;

    [SerializeField] private GameObject continueButtonActive;
    [SerializeField] private GameObject continueButtonInactive;

    private void Awake()
    {
        Game_Manager.instance.LoadSettings();
    }

    private void Start()
    {
        if (Game_Manager.instance.langChoosen)
        {
            menuObject.SetActive(true);
            langObject.SetActive(false);
        }

        else
        {
            menuObject.SetActive(false);
            langObject.SetActive(true);
        }
        
        // Temporarily remove listeners to avoid triggering toggle methods
        musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
        soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
        subtitlesToggle.onValueChanged.RemoveListener(OnSubtitlesToggleChanged);

        // Set toggle values without triggering events
        musicToggle.isOn = Game_Manager.instance.isMusicOn;
        soundToggle.isOn = Game_Manager.instance.isSoundOn;
        subtitlesToggle.isOn = Game_Manager.instance.areSubtitlesOn;

        // Re-attach listeners
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        subtitlesToggle.onValueChanged.AddListener(OnSubtitlesToggleChanged);

        // Update the continue button visibility based on passed rooms
        UpdateContinueButtonVisibility();
    }

    // Called when the "New Game" button is pressed
    public void NewGame()
    {
        // Reset room progress
        Game_Manager.instance.ResetAllRoomProgress();

        // Optionally clear PlayerPrefs if you want to start fresh
        PlayerPrefs.DeleteAll();

        // Load the intro scene or any starting scene
        SceneManager.LoadScene("Intro");
    }

    // Called when the language is chosen
    public void ChoseLang()
    {
        menuObject.SetActive(true);
        langObject.SetActive(false);
        Game_Manager.instance.langChoosen = true;
    }

    // Called when the "Continue" button is pressed
    public void ContinueGame()
    {
        Game_Manager.instance.ContinueGame();
    }

    // Updates the visibility of the continue button
    private void UpdateContinueButtonVisibility()
    {
        if (Game_Manager.instance.roomsPassed > 0)
        {
            continueButtonActive.SetActive(true);
            continueButtonInactive.SetActive(false);
        }
        else
        {
            continueButtonActive.SetActive(false);
            continueButtonInactive.SetActive(true);
        }
    }

    private void OnMusicToggleChanged(bool value)
    {
        Game_Manager.instance.SetMusic(value);
    }

    private void OnSoundToggleChanged(bool value)
    {
        Game_Manager.instance.SetSound(value);
    }

    private void OnSubtitlesToggleChanged(bool value)
    {
        Game_Manager.instance.SetSubtitles(value);
    }

    public void ToggleMusic()
    {
        bool newValue = !Game_Manager.instance.isMusicOn;
        Game_Manager.instance.SetMusic(newValue);
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
    }

    public void ToggleSound()
    {
        bool newValue = !Game_Manager.instance.isSoundOn;
        Game_Manager.instance.SetSound(newValue);
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
    }

    public void ToggleSubtitles()
    {
        bool newValue = !Game_Manager.instance.areSubtitlesOn;
        Game_Manager.instance.SetSubtitles(newValue);
        Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
    }

    public void OpenURL(string URL)
    {
        Application.OpenURL(URL);
    }
}
