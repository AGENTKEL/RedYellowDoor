using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        if (_audioSource != null)
        {
            _audioSource.volume = 0.1f;
            StartCoroutine(GraduallyIncreaseVolume());
        }
    }

    public void TriggerDeath()
    {
        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUI playerUI = other.GetComponent<PlayerUI>();
            if (playerUI != null)
            {
                playerUI.TriggerBlackScreenAndDie();
            }
        }
    }

    private IEnumerator GraduallyIncreaseVolume()
    {
        float duration = 30f; // 30 seconds
        float startVolume = 0.1f;
        float targetVolume = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        _audioSource.volume = targetVolume; // make sure it finishes exactly at 1
    }
}
