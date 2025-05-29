using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    private void Start()
    {
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
        Game_Manager.instance.AdjustGameObjectsForLocalization();
        Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
    }

    public void LoadBeginScene()
    {
        SceneManager.LoadScene("Begin");
    }
    
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
        Game_Manager.instance.ResetAllRoomProgress();
    }
}
