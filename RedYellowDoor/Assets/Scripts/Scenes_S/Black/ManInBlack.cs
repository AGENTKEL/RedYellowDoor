using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ManInBlack : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform playerTransform;
    public float killDistance = 1.5f;
    public AudioSource killAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource additionalAudioSource; // <- New: For triggered sound

    private PlayerUI playerUI;
    private Camera playerCamera;
    private bool hasKilledPlayer = false;

    // --- New variables for speed scaling ---
    [SerializeField] private float speedStart = 1f;
    [SerializeField] private float speedEnd = 5f;
    private float speedDuration = 30f;
    private float speedTimer = 0f;

    private void Start()
    {
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerTransform != null)
        {
            playerUI = playerTransform.GetComponent<PlayerUI>();
            playerCamera = playerTransform.GetComponentInChildren<Camera>();
        }

        if (agent != null)
        {
            agent.speed = speedStart; // Start at initial speed
        }

        if (musicAudioSource != null)
            musicAudioSource.Play();
    }

    private void Update()
    {
        if (hasKilledPlayer) return;

        // --- Gradually increase speed ---
        if (speedTimer < speedDuration)
        {
            speedTimer += Time.deltaTime;
            float t = Mathf.Clamp01(speedTimer / speedDuration); // goes from 0 to 1
            agent.speed = Mathf.Lerp(speedStart, speedEnd, t);
        }

        agent.SetDestination(playerTransform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= killDistance)
        {
            hasKilledPlayer = true;
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        agent.isStopped = true; // Stop moving

        if (killAudioSource != null)
            killAudioSource.Play();
        playerUI.playerController.isDead = true;

        if (playerCamera != null)
        {
            Vector3 offset = new Vector3(0, 1.8f, 0); // Look slightly above ManInBlack
            Vector3 lookDirection = ((transform.position + offset) - playerCamera.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            float elapsed = 0f;
            float rotationDuration = 0.2f;

            Quaternion startRotation = playerCamera.transform.rotation;

            while (elapsed < rotationDuration)
            {
                elapsed += Time.deltaTime;
                playerCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / rotationDuration);
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f);
        playerUI.TriggerBlackScreenAndDie();
    }

    public void PlayAdditionalSound()
    {
        if (additionalAudioSource != null && !additionalAudioSource.isPlaying)
        {
            additionalAudioSource.Play();
        }
    }
}
