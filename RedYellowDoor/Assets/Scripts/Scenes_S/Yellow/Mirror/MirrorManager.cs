using System;
using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    [Header("Mirror Info")]
    public int totalMirrors = 0;
    private int brokenMirrors = 0;

    [Header("Optional Key Logic")]
    public GameObject keyPrefab;
    public Transform keySpawnPoint;

    private void Start()
    {
        StartCoroutine(FindDoorCE());
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);
        
        FindFirstObjectByType<Door>().isLocked = true;
    }


    public void MirrorBroken(Transform brokenMirrorTransform)
    {
        brokenMirrors++;

        if (brokenMirrors >= totalMirrors)
        {
            SpawnKey(brokenMirrorTransform);
        }
    }

    private void SpawnKey(Transform spawnTransform)
    {
        if (keyPrefab && spawnTransform)
        {
            Vector3 spawnPosition = spawnTransform.position + new Vector3(0, 1.2f, 0);
            Instantiate(keyPrefab, spawnPosition, spawnTransform.rotation);
            Debug.Log("Key has spawned from the last mirror!");
        }
    }
}
