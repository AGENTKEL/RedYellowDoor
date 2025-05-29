using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class ManequinManager : MonoBehaviour
{
    private Door _door;
    
    public GameObject keyPrefab; // Assign your key prefab here
    public Transform[] keySpawnPoints; // Assign 4 spawn points in inspector

    private void Start()
    {
        StartCoroutine(FindDoorCE());
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);

        _door = FindFirstObjectByType<Door>();
        _door.isLocked = true;
        
        SpawnKeyAtRandomPoint();
    }
    
    private void SpawnKeyAtRandomPoint()
    {
        if (keySpawnPoints.Length == 0 || keyPrefab == null)
        {
            Debug.LogWarning("Key prefab or spawn points not assigned.");
            return;
        }

        int randomIndex = Random.Range(0, keySpawnPoints.Length);
        Transform spawnPoint = keySpawnPoints[randomIndex];

        Instantiate(keyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    
}
