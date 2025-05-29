using System;
using System.Collections;
using System.Collections.Generic;
using CharacterScript;
using DoorScript;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject deadScreen;
    public FPSController playerController;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Image blackImage;
    private bool isPaused;
    private Door _door;
    
    private bool waitingForRewardedAdToComplete = false;

    private void Start()
    {
        _door = FindFirstObjectByType<Door>();
    }

    public void PlayerDeath()
    {
        playerController.isDead = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        deadScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    
    public void ToMenuLost()
    {
        Time.timeScale = 1;
        Game_Manager.instance.ResetAllRoomProgress();
        SceneManager.LoadScene("Menu");
    }

    public void PlayAd()
    {
        Time.timeScale = 1;
        Game_Manager.instance._rewardedController.ShowTheAd(RestartScene);
    }

    public void SkipForAd()
    {
        Time.timeScale = 1;
        Game_Manager.instance._rewardedController.ShowTheAd(ProceedRoomSkipping);
    }

    private void ProceedRoomSkipping()
    {
            Time.timeScale = 1;

            if (SceneManager.GetActiveScene().name == "End")
            {
                Game_Manager.instance.SetLastScene("Cycle_Room");
                SceneManager.LoadScene("Cycle_Room");
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Black")
            {
                Game_Manager.instance.PassBlackRoom();
                if (Game_Manager.instance.blackRoomsPassed >= 3)
                {
                    SceneManager.LoadScene("Black_End");
                    return;
                }
                _door = FindFirstObjectByType<Door>();
                Game_Manager.instance.OnDoorEntered2(_door.doorColor);
                return;
            }
            
            if (Game_Manager.instance.roomsPassed >= 10)
            {
                Game_Manager.instance.SetLastScene("End");
                SceneManager.LoadScene("End");
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Begin")
            {
                Game_Manager.instance.yellowRoomsPassed++;
                Game_Manager.instance.roomsPassed++;
                _door = FindFirstObjectByType<Door>();
                Game_Manager.instance.OnDoorEntered2(_door.doorColor);
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Yellow")
            {
                Game_Manager.instance.roomsPassed++;
                Game_Manager.instance.yellowRoomsPassed++;
                _door = FindFirstObjectByType<Door>();
                Game_Manager.instance.OnDoorEntered2(_door.doorColor);
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Red")
            {
                Game_Manager.instance.roomsPassed++;
                Game_Manager.instance.redRoomsPassed++;
                _door = FindFirstObjectByType<Door>();
                Game_Manager.instance.OnDoorEntered2(_door.doorColor);
                return;
            }
            
            
            
            Game_Manager.instance.OnDoorEntered2(_door.doorColor);
    }
    

    // Pause
    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
            isPaused = false;
        }
        else
        {
            pauseScreen.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Yellow")
        {
            // Keep Yellow prefab index
            Debug.Log("Restarting Yellow scene, keeping lastYellowRoomIndex.");
        }
        else if (currentScene == "Red")
        {
            // Keep Red prefab index
            Debug.Log("Restarting Red scene, keeping lastRedRoomIndex.");
        }
        else
        {
            // Clear both in case of unrelated scene
            Game_Manager.instance.lastYellowRoomIndex = null;
            Game_Manager.instance.lastRedRoomIndex = null;
        }
        
        SceneManager.LoadScene(currentScene);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void TriggerBlackScreenAndDie()
    {
        StartCoroutine(FadeInBlackScreen());
    }

    private IEnumerator FadeInBlackScreen()
    {
        float fadeDuration = 1.5f; // Duration of the fade (in seconds)
        float elapsedTime = 0f;

        Color color = blackImage.color;
        color.a = 0f;
        blackImage.color = color;
        blackImage.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            blackImage.color = color;
            yield return null;
        }

        // After fade is complete, call PlayerDeath
        PlayerDeath();
    }
    
    public void TriggerBlackScreenAndSwitchScene()
    {
        StartCoroutine(FadeInBlackScreenSwitchScene());
    }

    private IEnumerator FadeInBlackScreenSwitchScene()
    {
        float fadeDuration = 3f; // Duration of the fade (in seconds)
        float elapsedTime = 0f;

        Color color = blackImage.color;
        color.a = 0f;
        blackImage.color = color;
        blackImage.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            blackImage.color = color;
            yield return null;
        }

        PlayerDeath();
    }
    
    public void OpenURL(string URL)
    {
        Application.OpenURL(URL);
    }
}
