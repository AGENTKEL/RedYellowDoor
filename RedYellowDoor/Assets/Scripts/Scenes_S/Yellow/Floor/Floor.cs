using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Floor : MonoBehaviour
{
    public Transform teleportTarget;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public GameObject player;

    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTeleporting && other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndTeleport());
        }
    }

    private IEnumerator FadeAndTeleport()
    {
        isTeleporting = true;

        // Step 1: Get references
        var controller = player.GetComponent<CharacterController>();
        var fps = player.GetComponent<CharacterScript.FPSController>();
        if (controller == null || fps == null)
        {
            Debug.LogError("Missing CharacterController or FPSController.");
            yield break;
        }

        // Step 2: Disable movement
        fps.canMove = false;

        // Step 3: Fade in
        yield return StartCoroutine(FadeImage(0f, 1f));

        // Step 4: Wait until movement stops
        yield return new WaitForEndOfFrame();

        // Step 5: Disable CharacterController before changing position
        controller.enabled = false;
        player.transform.position = teleportTarget.position;
        Debug.Log($"Teleported to {teleportTarget.position}");
        controller.enabled = true;

        // Optional tiny delay to stabilize
        yield return new WaitForSeconds(0.05f);

        // Step 6: Re-enable movement
        fps.canMove = true;

        // Step 7: Fade out
        yield return StartCoroutine(FadeImage(1f, 0f));

        isTeleporting = false;
    }

    private IEnumerator FadeImage(float from, float to)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, to); // ensure final value
    }
}
