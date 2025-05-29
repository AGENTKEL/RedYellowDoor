using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRandomizer : MonoBehaviour
{
    [Header("Fake Door Prefabs (Random Types)")]
    public GameObject[] fakeDoorPrefabs; // Different types to randomly pick from

    [Header("Positions for All Doors")]
    public Transform[] doorPositions;    // One will be used for real door, rest for fakes

    [Header("Real Door in Scene")]
    public GameObject realDoor;

    void Start()
    {
        RandomizeDoors();
    }

    void RandomizeDoors()
    {
        if (doorPositions.Length < 2)
        {
            Debug.LogError("You need at least 2 positions: one for real door, one for fake.");
            return;
        }

        // Convert to a mutable list
        List<Transform> availablePositions = new List<Transform>(doorPositions);

        // 1. Choose a random position for the real door
        int realDoorIndex = Random.Range(0, availablePositions.Count);
        Transform realDoorPos = availablePositions[realDoorIndex];
        realDoor.transform.position = realDoorPos.position;
        realDoor.transform.rotation = realDoorPos.rotation;
        availablePositions.RemoveAt(realDoorIndex);

        // 2. Spawn fake doors at remaining positions
        foreach (var pos in availablePositions)
        {
            // Pick a random prefab from the list
            GameObject randomFake = fakeDoorPrefabs[Random.Range(0, fakeDoorPrefabs.Length)];
            Instantiate(randomFake, pos.position, pos.rotation);
        }
    }
}
