using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoorScript
{
	public enum DoorColor
	{
		Yellow,
		Red,
		Black
	}

	[RequireComponent(typeof(AudioSource))]
	public class Door : MonoBehaviour
	{
		public bool open;
		public bool isLocked = true;
		public float smooth = 1.0f;
		private float DoorOpenAngle = -90.0f;
		private float DoorCloseAngle = 0.0f;

		public DoorColor doorColor;

		private AudioSource asource;
		public AudioClip openDoor, closeDoor;

		private void Start()
		{
			asource = GetComponent<AudioSource>();
		}

		private void Update()
		{
			float targetAngle = open ? DoorOpenAngle : DoorCloseAngle;
			Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 5 * smooth);
		}

		public void TryOpenWithKey(Inventory playerInventory)
		{
			if (isLocked)
			{
				if (playerInventory.UseItem(ItemType.Key))
				{
					Debug.Log("Door unlocked with a key!");
					isLocked = false;
					ToggleDoor();
				}
				else
				{
					Debug.Log("This door is locked. You need a key.");
				}
			}
			else
			{
				ToggleDoor();
			}
		}

		public void ToggleDoor()
		{
			open = !open;
			asource.clip = open ? openDoor : closeDoor;
			asource.Play();
		}
		
	}
}