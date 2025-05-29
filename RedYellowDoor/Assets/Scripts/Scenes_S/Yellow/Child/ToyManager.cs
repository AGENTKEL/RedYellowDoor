using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class ToyManager : MonoBehaviour
{
    private int giftedToys = 0;
    private int totalToys = 3; // Adjust if dynamic
    
    private void Start()
    {
        StartCoroutine(FindDoorCE());
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);
        
        FindFirstObjectByType<Door>().isLocked = true;
    }

    public void OnToyGifted(Toy toy)
    {
        giftedToys++;

        if (giftedToys >= totalToys)
        {
            Vector3 spawnPosition = toy.transform.position + Vector3.forward * 0.2f;
            Instantiate(toy.keyPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("All toys have been gifted!");
        }
    }
}
