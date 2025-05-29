using UnityEngine;

public class DoorW : MonoBehaviour
	{
		public bool open;
		public bool isLocked = true;
		public float smooth = 1.0f;
		[SerializeField] private float DoorOpenAngle = 90.0f;
		[SerializeField] private float DoorCloseAngle = 0.0f;

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
		

		public void ToggleDoor()
		{
			open = !open;
			asource.clip = open ? openDoor : closeDoor;
			asource.Play();
		}
		
	}
