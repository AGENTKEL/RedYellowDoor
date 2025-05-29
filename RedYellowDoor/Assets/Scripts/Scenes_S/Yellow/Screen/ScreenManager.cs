using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenManager : MonoBehaviour
{
    public Sprite[] screenSprites;
    public Material screenMaterial;
    public float changeInterval = 3f;

    public AudioClip doorUnlockClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource ambientAudioSource;
    public AudioClip ambientClip;

    [Header("Post Processing")]
    public Volume globalVolume;

    [Header("Camera Logic")]
    public Transform playerCamera;           // 🔥 Assign player's camera transform
    public Transform screenLookTarget;       // 🔥 A transform on the screen object (e.g., empty GameObject centered on screen)

    private FilmGrain filmGrain;
    private ColorAdjustments colorAdjustments;

    private Door _door;
    private DoorBlack _doorBlack;
    private float timer = 0f;
    private bool isLooking = false;
    private bool doorUnlocked = false;

    private int switchesCount = 0;
    private int switchesNeeded = 5;
    private List<Sprite> availableSprites = new List<Sprite>();

    private Coroutine fadeInCoroutine;
    private Coroutine postProcessingCoroutine;
    private Coroutine cameraShakeCoroutine;

    private Vector3 cameraOriginalPosition;

    private void Start()
    {
        StartCoroutine(FindDoorCE());

        screenMaterial.mainTexture = null;
        availableSprites = new List<Sprite>(screenSprites);

        if (globalVolume.profile.TryGet(out filmGrain))
            filmGrain.intensity.overrideState = true;

        if (globalVolume.profile.TryGet(out colorAdjustments))
            colorAdjustments.postExposure.overrideState = true;
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);

        _door = FindFirstObjectByType<Door>();
        _doorBlack = FindFirstObjectByType<DoorBlack>();
        if (_door != null)
            _door.isLocked = true;
    }

    private void Update()
    {
        if (!isLooking || doorUnlocked) return;

        timer += Time.deltaTime;

        // 🔥 Always rotate camera to look at screen target
        if (playerCamera != null && screenLookTarget != null)
        {
            Vector3 direction = screenLookTarget.position - playerCamera.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, Time.deltaTime * 5f);
        }

        if (timer >= changeInterval)
        {
            timer = 0f;

            if (switchesCount == 0)
            {
                StartEffects();
            }

            if (availableSprites.Count > 0)
            {
                int randomIndex = Random.Range(0, availableSprites.Count);
                Sprite sprite = availableSprites[randomIndex];

                if (sprite != null && sprite.texture != null)
                    screenMaterial.mainTexture = sprite.texture;

                availableSprites.RemoveAt(randomIndex);
                switchesCount++;

                if (switchesCount >= switchesNeeded)
                {
                    UnlockDoor();
                }
            }
        }
    }

    private void StartEffects()
    {
        if (ambientAudioSource != null && ambientClip != null)
        {
            ambientAudioSource.clip = ambientClip;
            ambientAudioSource.volume = 0f;
            ambientAudioSource.loop = true;
            ambientAudioSource.Play();

            fadeInCoroutine = StartCoroutine(FadeInAmbientAudio(20f));
        }

        postProcessingCoroutine = StartCoroutine(FadeInPostProcessing(20f));
        cameraShakeCoroutine = StartCoroutine(CameraShake(20f));
        cameraOriginalPosition = playerCamera.localPosition;
    }

    private IEnumerator FadeInAmbientAudio(float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            ambientAudioSource.volume = Mathf.Lerp(0f, 1f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        ambientAudioSource.volume = 1f;
    }

    private IEnumerator FadeInPostProcessing(float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            if (filmGrain != null)
                filmGrain.intensity.value = Mathf.Lerp(0f, 1f, t);

            if (colorAdjustments != null)
                colorAdjustments.postExposure.value = Mathf.Lerp(0.7f, 0f, t);

            time += Time.deltaTime;
            yield return null;
        }

        if (filmGrain != null)
            filmGrain.intensity.value = 1f;

        if (colorAdjustments != null)
            colorAdjustments.postExposure.value = 0f;
    }

    private IEnumerator CameraShake(float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            float intensity = Mathf.Lerp(0f, 0.15f, time / duration);
            if (playerCamera != null)
            {
                Vector3 shakeOffset = Random.insideUnitSphere * intensity;
                playerCamera.localPosition = cameraOriginalPosition + shakeOffset;
            }

            time += Time.deltaTime;
            yield return null;
        }

        if (playerCamera != null)
            playerCamera.localPosition = cameraOriginalPosition;
    }

    private void UnlockDoor()
    {
        doorUnlocked = true;

        if (_door != null)
            _door.isLocked = false;
        _doorBlack.UnlockBlackDoor();

        if (audioSource != null && doorUnlockClip != null)
            audioSource.PlayOneShot(doorUnlockClip);

        if (ambientAudioSource != null && ambientAudioSource.isPlaying)
        {
            if (fadeInCoroutine != null)
                StopCoroutine(fadeInCoroutine);
            ambientAudioSource.Stop();
        }

        if (postProcessingCoroutine != null)
            StopCoroutine(postProcessingCoroutine);

        if (cameraShakeCoroutine != null)
            StopCoroutine(cameraShakeCoroutine);

        StartCoroutine(ResetPostProcessing(1f));

        if (playerCamera != null)
            playerCamera.localPosition = cameraOriginalPosition;

        Debug.Log("Door unlocked after switching 5 images!");
    }

    private IEnumerator ResetPostProcessing(float duration)
    {
        float time = 0f;
        float startGrain = filmGrain.intensity.value;
        float startExposure = colorAdjustments.postExposure.value;

        while (time < duration)
        {
            float t = time / duration;

            if (filmGrain != null)
                filmGrain.intensity.value = Mathf.Lerp(startGrain, 0f, t);

            if (colorAdjustments != null)
                colorAdjustments.postExposure.value = Mathf.Lerp(startExposure, 0.7f, t);

            time += Time.deltaTime;
            yield return null;
        }

        if (filmGrain != null)
            filmGrain.intensity.value = 0f;

        if (colorAdjustments != null)
            colorAdjustments.postExposure.value = 0.7f;
    }

    public void SetLooking(bool value)
    {
        if (isLooking != value)
        {
            isLooking = value;

            if (!isLooking)
            {
                timer = 0f;
            }
        }
    }
}
