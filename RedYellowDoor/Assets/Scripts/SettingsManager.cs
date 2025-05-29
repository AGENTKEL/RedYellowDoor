using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle subtitlesToggle;

    private void Awake()
    {
        Game_Manager.instance.LoadSettings();
    }

    private void Start()
    {
        UpdateToggles();
    }

    public void UpdateToggles()
    {
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
    }
    
    private void OnMusicToggleChanged(bool value)
    {
        Game_Manager.instance.SetMusic(value);
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
    }

    private void OnSoundToggleChanged(bool value)
    {
        Game_Manager.instance.SetSound(value);
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
    }

    private void OnSubtitlesToggleChanged(bool value)
    {
        Game_Manager.instance.SetSubtitles(value);
        Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
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
}
