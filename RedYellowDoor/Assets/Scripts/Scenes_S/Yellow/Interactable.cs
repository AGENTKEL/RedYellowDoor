using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Key, Note, NoteCar, Toy, Gift, NoteBlack }

[RequireComponent(typeof(AudioSource))]
public class Interactable : MonoBehaviour
{
    public ItemType itemType;
    public Sprite itemSprite;

    [Header("Audio")]
    public AudioClip interactClip; // 🔊 Clip to play for this interactable

    private Audio audioManager;    // 🔊 Reference to central audio manager
    private bool isMovingToTarget = false;
    private bool isHeldByPlayer = false;
    private Transform target;

    public float moveSpeed = 5f;

    void Start()
    {
        audioManager = FindFirstObjectByType<Audio>();
        if (audioManager == null)
        {
            Debug.LogWarning("No Audio script found in the scene. Please add one to a GameObject.");
        }
    }

    void Update()
    {
        if (isMovingToTarget && target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(transform.position, target.position) < 0.01f)
            {
                transform.SetParent(target);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                isMovingToTarget = false;
                isHeldByPlayer = true;
            }
        }
    }

    public void MoveTo(Transform newTarget)
    {
        if (!isHeldByPlayer)
        {
            target = newTarget;
            isMovingToTarget = true;
        }
        else
        {
            Destroy(gameObject); // Second interaction destroys it
        }
    }

    public void Interact(Inventory inventory)
    {
        // 🔊 Play central interaction audio
        if (interactClip != null && audioManager != null)
        {
            audioManager.PlayClip(interactClip);
        }

        if (itemType == ItemType.Toy)
        {
            Toy toy = GetComponent<Toy>();
            toy.Interact(inventory);
            return;
        }

        if (itemType == ItemType.Note)
        {
            MoveTo(inventory.playerCameraInteract.noteTarget);
        }
        else if (itemType == ItemType.NoteCar)
        {
            MoveTo(inventory.playerCameraInteract.noteTarget);
            FindFirstObjectByType<CarManager>().UnlockDoor();
        }
        else if (itemType == ItemType.NoteBlack)
        {
            MoveTo(inventory.playerCameraInteract.noteTarget);
            FindFirstObjectByType<BlackManager>().UnlockDoor();
        }
        else
        {
            inventory.AddItem(this);
        }
    }
}
