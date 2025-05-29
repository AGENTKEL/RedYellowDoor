using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackTrigger : MonoBehaviour
{
    public GameObject timelines;
    public GameObject goBackTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timelines.SetActive(true);
            goBackTrigger.SetActive(true);
            Game_Manager.instance.AdjustGameObjectsForSoundSettings();
            Game_Manager.instance.AdjustGameObjectsForLocalization();
            Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
        }
    }
}
