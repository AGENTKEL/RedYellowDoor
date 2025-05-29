using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ManInBlack1 : MonoBehaviour
{
     public NavMeshAgent agent;
    public Transform playerTransform;
    public float killDistance = 1.5f;
    public AudioSource killAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource additionalAudioSource; // For triggered sound

    private PlayerUI playerUI;
    private Camera playerCamera;
    private bool hasKilledPlayer = false;

    // --- Speed scaling ---
    [SerializeField] private float speedStart = 1f;
    [SerializeField] private float speedEnd = 5f;
    private float speedDuration = 40f;
    private float speedTimer = 0f;

    // --- New: Random Audio ---
    [Header("Random Audio Settings")]
    public List<AudioClip> randomAudioClips; // Assign your clips in Inspector
    public float minInterval = 5f; // Minimum seconds between sounds
    public float maxInterval = 15f; // Maximum seconds between sounds

    private AudioSource randomAudioSource;

    private void Start()
    {
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
        Game_Manager.instance.AdjustGameObjectsForLocalization();
        Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
        
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerTransform != null)
        {
            playerUI = playerTransform.GetComponent<PlayerUI>();
            playerCamera = playerTransform.GetComponentInChildren<Camera>();
        }

        if (agent != null)
        {
            agent.speed = speedStart;
        }

        if (musicAudioSource != null)
            musicAudioSource.Play();

        // --- Random Audio Source setup ---
        randomAudioSource = gameObject.AddComponent<AudioSource>();
        randomAudioSource.playOnAwake = false;
        randomAudioSource.spatialBlend = 0.7f; // Make it 3D sound
        StartCoroutine(PlayRandomAudioLoop());
    }

    private void Update()
    {
        if (hasKilledPlayer) return;

        // Speed scaling
        if (speedTimer < speedDuration)
        {
            speedTimer += Time.deltaTime;
            float t = Mathf.Clamp01(speedTimer / speedDuration);
            agent.speed = Mathf.Lerp(speedStart, speedEnd, t);
        }

        if (playerTransform != null)
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
        agent.isStopped = true;

        if (killAudioSource != null)
            killAudioSource.Play();
        playerUI.playerController.isDead = true;

        if (playerCamera != null)
        {
            Vector3 offset = new Vector3(0, 1.8f, 0); // Look slightly above ManInBlack
            Vector3 lookDirection = ((transform.position + offset) - playerCamera.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            float elapsed = 0f;
            float rotationDuration = 0.3f;

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

    // --- New: Random Audio Playing Loop ---
    private IEnumerator PlayRandomAudioLoop()
    {
        while (!hasKilledPlayer) // Only while player is alive
        {
            if (randomAudioClips != null && randomAudioClips.Count > 0)
            {
                AudioClip clip = randomAudioClips[Random.Range(0, randomAudioClips.Count)];
                if (clip != null)
                {
                    randomAudioSource.clip = clip;
                    randomAudioSource.Play();
                }
            }

            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
