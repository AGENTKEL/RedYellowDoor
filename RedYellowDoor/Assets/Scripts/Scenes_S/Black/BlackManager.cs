using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class BlackManager : MonoBehaviour
{
    private Door _door;
    [SerializeField] private GameObject timeline;
    [SerializeField] private GameObject manInBlack;
    
    private void Start()
    {
        StartCoroutine(FindDoorCE());
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);

        _door = FindFirstObjectByType<Door>();
        _door.isLocked = true;
    }

    public void UnlockDoor()
    {
        timeline.SetActive(true);
        manInBlack.SetActive(true);
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
        _door.isLocked = false;
    }
}
