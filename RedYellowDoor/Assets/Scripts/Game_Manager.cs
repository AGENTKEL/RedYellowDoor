using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance = null;

    public int roomsPassed = 0; // Includes red & yellow only
    public int redRoomsPassed = 0;
    public int yellowRoomsPassed = 0;
    public int blackRoomsPassed = 0;
    public int blackDoorUses = 0;

    private bool justExitedBlackDoor = false;

    [Header("Scene Names")]
    public string yellowRoomScene = "Yellow";
    public string redRoomScene = "Red";
    public string blackRoomScene = "Black";

    [Header("Passed Level Indexes")]
    public List<int> yellowRoomsUsed = new List<int>();
    public List<int> redRoomsUsed = new List<int>();

    public int? lastYellowRoomIndex = null;
    public int? lastRedRoomIndex = null;
    
    [Header("Settings Toggles")]
    public bool isMusicOn = true;
    public bool isSoundOn = true;
    public bool areSubtitlesOn = true;
    private const string MusicKey = "Music";
    private const string SoundKey = "Sound";
    private const string SubtitlesKey = "Subtitles";
    
    [Header("Language Settings")]
    public bool langChoosen = false;

    // Keys for room progress
    private const string RedRoomsPassedKey = "RedRoomsPassed";
    private const string YellowRoomsPassedKey = "YellowRoomsPassed";
    private const string BlackRoomsPassedKey = "BlackRoomsPassed";
    private const string BlackDoorUsesKey = "BlackDoorUses";
    private const string RoomsPassedKey = "RoomsPassed";
    
    private const string LastSceneNameKey = "LastSceneName";
    public string lastSceneName = "";

    public InterstitialController _interstitialController;
    public RewardedController _rewardedController;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
            LoadRoomProgress(); // Load saved room progress
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        if (PlayerPrefs.HasKey(LastSceneNameKey))
        {
            lastSceneName = PlayerPrefs.GetString(LastSceneNameKey);
        }
    }
    
    
    public void SetLastScene(string sceneName)
    {
        lastSceneName = sceneName;
        PlayerPrefs.SetString(LastSceneNameKey, lastSceneName);
        PlayerPrefs.Save();
    }
    
    public void ContinueGame()
    {
        if (!string.IsNullOrEmpty(lastSceneName))
        {
            // Restore room index based on which scene we're continuing into
            if (lastSceneName == "Yellow")
            {
                Debug.Log("Continuing into Yellow scene, restoring lastYellowRoomIndex.");
                // Do nothing – it should already be preserved from previous session or load logic
            }
            else if (lastSceneName == "Red")
            {
                Debug.Log("Continuing into Red scene, restoring lastRedRoomIndex.");
                // Same – preserved or loaded via PlayerPrefs if needed
            }
            else
            {
                // Clear room indices if continuing to an unrelated scene
                Game_Manager.instance.lastYellowRoomIndex = null;
                Game_Manager.instance.lastRedRoomIndex = null;
            }

            Time.timeScale = 1;
            SceneManager.LoadScene(lastSceneName);
        }
        else
        {
            Debug.LogWarning("No last scene name found. Loading default scene.");
            SceneManager.LoadScene("Menu");
        }
    }



    public void OnDoorEntered(DoorScript.DoorColor doorColor)
    {
        switch (doorColor)
        {
            case DoorScript.DoorColor.Black:
                HandleBlackDoor();
                break;
            case DoorScript.DoorColor.Red:
                HandleRedRoom();
                break;
            case DoorScript.DoorColor.Yellow:
                HandleYellowRoom();
                break;
        }
        if (SceneManager.GetActiveScene().name == "Yellow")
            lastYellowRoomIndex = null;
        else if (SceneManager.GetActiveScene().name == "Red")
            lastRedRoomIndex = null;
    }
    
    public void OnDoorEntered2(DoorScript.DoorColor doorColor)
    {
        switch (doorColor)
        {
            case DoorScript.DoorColor.Black:
                HandleBlackDoor2();
                break;
            case DoorScript.DoorColor.Red:
                HandleRedRoom2();
                break;
            case DoorScript.DoorColor.Yellow:
                HandleYellowRoom2();
                break;
        }
        if (SceneManager.GetActiveScene().name == "Yellow")
            lastYellowRoomIndex = null;
        else if (SceneManager.GetActiveScene().name == "Red")
            lastRedRoomIndex = null;
    }
    

    private void HandleYellowRoom()
    {
        justExitedBlackDoor = false;
        SaveRoomProgress();
        
        _interstitialController.OnRoomPassed(() => {
            // Code to load next level here
            SceneManager.LoadScene(yellowRoomScene);
            return;
        });
        SceneManager.LoadScene(yellowRoomScene);
    }
    
    private void HandleYellowRoom2()
    {
        justExitedBlackDoor = false;
        SaveRoomProgress();
        
        SceneManager.LoadScene(yellowRoomScene);
    }

    private void HandleRedRoom()
    {
        justExitedBlackDoor = false;
        SaveRoomProgress(); // Save progress after passing a room
        
        _interstitialController.OnRoomPassed(() => {
            // Code to load next level here
            SceneManager.LoadScene(redRoomScene);
            return;
        });
        
        SceneManager.LoadScene(redRoomScene);
    }
    
    private void HandleRedRoom2()
    {
        justExitedBlackDoor = false;
        SaveRoomProgress(); // Save progress after passing a room

        SceneManager.LoadScene(redRoomScene);
    }

    private void HandleBlackDoor()
    {
        blackDoorUses++;
        justExitedBlackDoor = true;
        SaveRoomProgress();
        SceneManager.LoadScene(blackRoomScene);
        
    }
    
    private void HandleBlackDoor2()
    {
        blackDoorUses++;
        justExitedBlackDoor = true;
        SaveRoomProgress(); // Save progress after using a black door
        _interstitialController.OnRoomPassed(() => {
            // Code to load next level here
            SceneManager.LoadScene(blackRoomScene);
        });
        
        SceneManager.LoadScene(blackRoomScene);
        
    }

    public void PassBlackRoom()
    {
        blackRoomsPassed++;
        SaveRoomProgress(); // Save progress after passing a black room
    }

    public void ReturnFromBlackRoom()
    {
        if (blackDoorUses < 3)
        {
            // We do NOT increment roomsPassed here, so red/yellow sequence stays intact
        }

        justExitedBlackDoor = false;
    }

    // New method to save the progress in PlayerPrefs
    private void SaveRoomProgress()
    {
        PlayerPrefs.SetInt(RoomsPassedKey, roomsPassed); // 🔥 NEW
        PlayerPrefs.SetInt(RedRoomsPassedKey, redRoomsPassed);
        PlayerPrefs.SetInt(YellowRoomsPassedKey, yellowRoomsPassed);
        PlayerPrefs.SetInt(BlackRoomsPassedKey, blackRoomsPassed);
        PlayerPrefs.SetInt(BlackDoorUsesKey, blackDoorUses);
        PlayerPrefs.Save();
    }

    // New method to load the saved room progress from PlayerPrefs
    private void LoadRoomProgress()
    {
        roomsPassed = PlayerPrefs.GetInt(RoomsPassedKey, 0); // 🔥 NEW
        redRoomsPassed = PlayerPrefs.GetInt(RedRoomsPassedKey, 0);
        yellowRoomsPassed = PlayerPrefs.GetInt(YellowRoomsPassedKey, 0);
        blackRoomsPassed = PlayerPrefs.GetInt(BlackRoomsPassedKey, 0);
        blackDoorUses = PlayerPrefs.GetInt(BlackDoorUsesKey, 0);
    }
    
    public void ResetAllRoomProgress()
    {
        roomsPassed = 0;
        redRoomsPassed = 0;
        yellowRoomsPassed = 0;
        blackRoomsPassed = 0;
        blackDoorUses = 0;
        
        yellowRoomsUsed.Clear();
        redRoomsUsed.Clear();
        
        Game_Manager.instance.lastYellowRoomIndex = null;
        Game_Manager.instance.lastRedRoomIndex = null;

        PlayerPrefs.SetInt(RoomsPassedKey, 0);
        PlayerPrefs.SetInt(RedRoomsPassedKey, 0);
        PlayerPrefs.SetInt(YellowRoomsPassedKey, 0);
        PlayerPrefs.SetInt(BlackRoomsPassedKey, 0);
        PlayerPrefs.SetInt(BlackDoorUsesKey, 0);
        PlayerPrefs.Save();
    }

    public DoorScript.DoorColor GetNextExpectedRoomColor()
    {
        if (justExitedBlackDoor)
            return DoorScript.DoorColor.Yellow;

        int cyclePosition = roomsPassed % 3;
        return cyclePosition == 2 ? DoorScript.DoorColor.Red : DoorScript.DoorColor.Yellow;
    }

    public DoorScript.DoorColor GetNextColorAfterBlackRoom()
    {
        int cyclePosition = roomsPassed % 3;
        return cyclePosition == 2 ? DoorScript.DoorColor.Red : DoorScript.DoorColor.Yellow;
    }

    public bool ShouldSpawnBlackDoor()
    {
        return blackDoorUses < 3 && (roomsPassed == 0 || roomsPassed == 3 || roomsPassed == 6);
    }

    public void ResetAllRoomTracking()
    {
        yellowRoomsUsed.Clear();
        redRoomsUsed.Clear();
        
        roomsPassed = 0;
        redRoomsPassed = 0;
        yellowRoomsPassed = 0;
        blackRoomsPassed = 0;
        blackDoorUses = 0;
    }
    
    public void SetMusic(bool value)
    {
        isMusicOn = value;
        PlayerPrefs.SetInt(MusicKey, value ? 1 : 0);
        PlayerPrefs.Save();
        AdjustGameObjectsForSoundSettings();
    }

    public void SetSound(bool value)
    {
        isSoundOn = value;
        PlayerPrefs.SetInt(SoundKey, value ? 1 : 0);
        PlayerPrefs.Save();
        AdjustGameObjectsForSoundSettings();
    }

    public void SetSubtitles(bool value)
    {
        areSubtitlesOn = value;
        PlayerPrefs.SetInt(SubtitlesKey, value ? 1 : 0);
        PlayerPrefs.Save();
        AdjustGameObjectsForSubtitlesSettings();
    }

    public void LoadSettings()
    {
        isMusicOn = PlayerPrefs.GetInt(MusicKey, 1) == 1;
        isSoundOn = PlayerPrefs.GetInt(SoundKey, 1) == 1;
        areSubtitlesOn = PlayerPrefs.GetInt(SubtitlesKey, 1) == 1;
    }
    
    public void AdjustGameObjectsForLocalization()
    {
        // Get the current localization index based on the selected locale
        int localizationIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);

        // Determine the tag based on the localization index
        string tag = localizationIndex switch
        {
            0 => "Eng",  // English
            1 => "Rus",  // Russian
            2 => "Esp",  // Spanish
            _ => "Eng",  // Default to English if not found
        };

        // Enable/Disable GameObjects based on localization
        GameObject[] localizedObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in localizedObjects)
        {
            obj.SetActive(true);
        }

        // Disable GameObjects for other languages
        string[] allTags = { "Eng", "Rus", "Esp" };
        foreach (var otherTag in allTags)
        {
            if (otherTag != tag)
            {
                GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag(otherTag);
                foreach (var obj in objectsToDisable)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
    
    public void AdjustGameObjectsForSoundSettings()
    {
        // Handle Music Objects
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("Music");
        foreach (var obj in musicObjects)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.enabled = isMusicOn;
            }
        }

        // Handle Audio Objects
        GameObject[] audioObjects = GameObject.FindGameObjectsWithTag("Audio");
        foreach (var obj in audioObjects)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.enabled = isSoundOn;
            }
        }
    }

    public void AdjustGameObjectsForSubtitlesSettings()
    {
        // Handle Subtitle Objects
        GameObject[] subtitleObjects = GameObject.FindGameObjectsWithTag("Sub");
        foreach (var obj in subtitleObjects)
        {
            Transform childTransform = obj.transform.GetChild(0);
            if (childTransform != null)
            {
                childTransform.gameObject.SetActive(areSubtitlesOn);
            }
        }
    }
}
