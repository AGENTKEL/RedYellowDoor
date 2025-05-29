using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraDoorScript
{
	public class CameraOpenDoor : MonoBehaviour
	{
		public float DistanceOpen = 3f;
		public GameObject text;
		public Inventory playerInventory;
		public Transform noteTarget;
		
		private ScreenManager currentlyLookedScreen;

		private Camera mainCam;
		
		private DoorScript.Door currentDoor;
		private DoorW currentDoorW;
		private Interactable currentInteractable;
		private Candle currentCandle;

		private void Start()
		{
			mainCam = Camera.main;
		}

		void Update()
		{
			Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
			RaycastHit hit;

			bool hitScreen = false;

			if (Physics.Raycast(ray, out hit, DistanceOpen))
			{
				var door = hit.transform.GetComponent<DoorScript.Door>();
				var doorW = hit.transform.GetComponent<DoorW>();
				var interactable = hit.transform.GetComponent<Interactable>();
				var candle = hit.transform.GetComponent<Candle>();
				var screen = hit.transform.GetComponent<ScreenManager>();

				// Reset all current targets every frame
				currentDoor = null;
				currentDoorW = null;
				currentInteractable = null;
				currentCandle = null;

				if (screen != null)
				{
					hitScreen = true;

					if (currentlyLookedScreen != screen)
					{
						if (currentlyLookedScreen != null)
							currentlyLookedScreen.SetLooking(false);

						screen.SetLooking(true);
						currentlyLookedScreen = screen;
					}
				}

				if (door != null)
				{
					text.SetActive(true);
					currentDoor = door;

					if (Input.GetKeyDown(KeyCode.E))
					{
						Interact();
					}
				}
				else if (doorW != null)
				{
					text.SetActive(true);
					currentDoorW = doorW;

					if (Input.GetKeyDown(KeyCode.E))
					{
						Interact();
					}
				}
				else if (interactable != null)
				{
					text.SetActive(true);
					currentInteractable = interactable;

					if (Input.GetKeyDown(KeyCode.E))
					{
						Interact();
					}
				}
				else if (candle != null)
				{
					text.SetActive(true);
					currentCandle = candle;

					if (Input.GetKeyDown(KeyCode.E))
					{
						Interact();
					}
				}
				else
				{
					text.SetActive(false);
				}
			}
			else
			{
				text.SetActive(false);

				// Reset current targets if looking at nothing
				currentDoor = null;
				currentDoorW = null;
				currentInteractable = null;
				currentCandle = null;
			}
		}
		
		public void Interact()
		{
			if (currentDoor != null)
			{
				currentDoor.TryOpenWithKey(playerInventory);
			}
			else if (currentDoorW != null)
			{
				currentDoorW.ToggleDoor();
			}
			else if (currentInteractable != null)
			{
				currentInteractable.Interact(playerInventory);
			}
			else if (currentCandle != null)
			{
				currentCandle.Interact();
			}
		}
	}
}
