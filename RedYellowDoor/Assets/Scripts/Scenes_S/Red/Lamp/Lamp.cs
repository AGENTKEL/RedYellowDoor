using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public AudioSource breakSound; // Assign a sound here
    public GameObject lightObject; // Assign the Light child here
    public Collider brokenCollider; // Assign the collider here

    private bool isBroken = false;

    public void Break()
    {
        if (isBroken) return;

        isBroken = true;

        if (breakSound != null)
            breakSound.Play();

        if (lightObject != null)
            lightObject.SetActive(false);

        if (brokenCollider != null)
            brokenCollider.enabled = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isBroken) return;

        if (other.CompareTag("Player"))
        {
            PlayerUI playerUI = other.GetComponent<PlayerUI>();
            if (playerUI != null)
            {
                playerUI.PlayerDeath();
            }
        }
    }
}
