using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private MirrorManager mirrorManager;
    public GameObject normalMirror; // Unbroken mirror
    public GameObject crackedMirror; // Broken mirror
    public AudioSource audioSource; // Crack sound

    private bool isBroken = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isBroken) return;

        if (other.CompareTag("Player"))
        {
            BreakMirror();
        }
    }

    private void BreakMirror()
    {
        isBroken = true;

        if (normalMirror) normalMirror.SetActive(false);
        if (crackedMirror) crackedMirror.SetActive(true);
        if (audioSource) audioSource.Play();

        mirrorManager.MirrorBroken(transform);
    }
}
