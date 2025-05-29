using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manequin : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;
    public float checkInterval = 0.2f;
    public float killDistance = 1f;

    private Camera playerCamera;
    private bool isVisible;
    private bool hasKilledPlayer = false;

    [SerializeField] private Transform[] visibilityPoints;
    private PlayerUI playerUI;
    private AudioSource audioManager;
    public AudioClip jumpscareClip;

    void Start()
    {
        playerCamera = Camera.main;
        StartCoroutine(CheckVisibilityRoutine());

        playerUI = FindFirstObjectByType<PlayerUI>();
        audioManager = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasKilledPlayer) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Kill player if too close
        if (distance <= killDistance)
        {
            hasKilledPlayer = true;
            StartCoroutine(JumpscareSequence());
            return;
        }

        if (!isVisible && distance > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    private IEnumerator CheckVisibilityRoutine()
    {
        while (true)
        {
            isVisible = IsAnyPointVisible();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    bool IsAnyPointVisible()
    {
        foreach (Transform point in visibilityPoints)
        {
            Vector3 viewPos = playerCamera.WorldToViewportPoint(point.position);

            if (viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1)
            {
                Vector3 direction = point.position - playerCamera.transform.position;
                Ray ray = new Ray(playerCamera.transform.position, direction);

                if (Physics.Raycast(ray, out RaycastHit hit, direction.magnitude + 0.1f))
                {
                    if (hit.transform == transform || hit.transform.IsChildOf(transform))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator JumpscareSequence()
    {
        // 1. Play sound
        if (audioManager != null && jumpscareClip != null)
        {
            audioManager.PlayOneShot(jumpscareClip);
        }

        playerUI.playerController.isDead = true;

        // 2. Make player's camera look at this Manequin
        float t = 0;
        float duration = 0.3f;
        Transform cam = playerCamera.transform;
        Quaternion initialRot = cam.rotation;
        Vector3 offset = new Vector3(0, 1.5f, 0); // Look slightly above the manequin
        Vector3 directionToManequin = ((transform.position + offset) - cam.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(directionToManequin);

        while (t < duration)
        {
            cam.rotation = Quaternion.Slerp(initialRot, targetRot, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        cam.rotation = targetRot;

        // 3. Shake camera
        PlayerCameraShaker shaker = cam.GetComponent<PlayerCameraShaker>();
        if (shaker != null)
        {
            shaker.StartShake(1.5f, 0.15f, 1f);
        }

        // 4. Wait 1 second
        yield return new WaitForSeconds(1.5f);

        // 5. Stop sound if needed
        if (audioManager != null)
        {
            audioManager.Stop();
        }

        // 6. Kill player
        if (playerUI != null)
        {
            playerUI.PlayerDeath();
        }
    }
}
