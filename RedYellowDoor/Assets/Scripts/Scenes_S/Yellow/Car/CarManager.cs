using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    private Door _door;
    private DoorBlack _doorBlack;
    [SerializeField] private GameObject timeline;
    
    private void Start()
    {
        StartCoroutine(FindDoorCE());
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);

        _door = FindFirstObjectByType<Door>();
        _doorBlack = FindFirstObjectByType<DoorBlack>();
        _door.isLocked = true;
    }

    public void UnlockDoor()
    {
        timeline.SetActive(true);
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
        Game_Manager.instance.AdjustGameObjectsForLocalization();
        Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
        _door.isLocked = false;
        _doorBlack.UnlockBlackDoor();
    }
}
