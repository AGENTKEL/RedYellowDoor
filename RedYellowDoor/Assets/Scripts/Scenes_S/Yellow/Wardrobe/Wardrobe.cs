using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    private Door _door;

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
}
