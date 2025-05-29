using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class DoorFake : MonoBehaviour
{
    public Door linkedDoor;
    public static List<DoorFake> allFakes = new List<DoorFake>();

    public Transform teleportPoint; // A child transform where player should appear after teleport
    public Vector3 teleportOffset = new Vector3(0, 0, 1f); // Optional small offset to prevent immediate re-trigger

    private void Awake()
    {
        allFakes.Add(this);
    }

    private void OnDestroy()
    {
        allFakes.Remove(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && linkedDoor.open)
        {
            linkedDoor.ToggleDoor();

            // Find a random DoorFake that isn't this one
            DoorFake targetFake = null;
            if (allFakes.Count > 1)
            {
                List<DoorFake> otherFakes = new List<DoorFake>(allFakes);
                otherFakes.Remove(this);
                targetFake = otherFakes[Random.Range(0, otherFakes.Count)];
            }

            if (targetFake != null && targetFake.teleportPoint != null)
            {
                CharacterController cc = other.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                other.transform.position = targetFake.teleportPoint.position + teleportOffset;
                other.transform.rotation = targetFake.teleportPoint.rotation;

                if (cc != null) cc.enabled = true;
            }
            else
            {
                Debug.LogWarning("No valid fake door to teleport to.");
            }
        }
    }
}
