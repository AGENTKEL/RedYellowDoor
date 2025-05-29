using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterScript
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        public float walkingSpeed = 7.5f;
        public float runningSpeed = 11.5f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public Camera playerCamera;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;

        CharacterController characterController;
        public Joystick joystick; // RIGHT side movement joystick
        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        [HideInInspector]
        public bool canMove = true;

        public bool isDead = false;

        private bool jumpRequest = false;

        public List<AudioClip> footstepClips;
        [SerializeField] private AudioSource audioSource;
        private float stepTimer = 0f;
        public float stepInterval = 0.4f;

        private int lookFingerId = -1;
        private Vector2 lastLookPosition;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (isDead) return;

            HandleMovement();
            HandleLook();
            HandleFootsteps();
        }

        private void HandleMovement()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float inputVertical = joystick != null ? joystick.Vertical : 0f;
            float inputHorizontal = joystick != null ? joystick.Horizontal : 0f;

            float curSpeedX = canMove ? walkingSpeed * inputVertical : 0;
            float curSpeedY = canMove ? walkingSpeed * inputHorizontal : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if ((jumpRequest) && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
                jumpRequest = false;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }

        private void HandleLook()
        {
            if (!canMove) return;

            int joystickFingerId = joystick != null ? joystick.JoystickFingerId : -1;

            foreach (Touch touch in Input.touches)
            {
                // Skip the joystick finger
                if (touch.fingerId == joystickFingerId)
                    continue;

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.deltaPosition;

                    rotationX -= delta.y * lookSpeed;
                    rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                    transform.rotation *= Quaternion.Euler(0, delta.x * lookSpeed, 0);
                }
            }
        }

        private void HandleFootsteps()
        {
            float inputVertical = joystick != null ? joystick.Vertical : 0f;
            float inputHorizontal = joystick != null ? joystick.Horizontal : 0f;

            bool isMoving = Mathf.Abs(inputVertical) > 0.1f || Mathf.Abs(inputHorizontal) > 0.1f;

            if (characterController.isGrounded && isMoving)
            {
                stepTimer += Time.deltaTime;
                if (stepTimer > stepInterval)
                {
                    PlayFootstep();
                    stepTimer = 0f;
                }
            }
            else
            {
                stepTimer = 0f;
            }
        }

        private void PlayFootstep()
        {
            if (footstepClips.Count == 0) return;

            int index = Random.Range(0, footstepClips.Count);
            audioSource.PlayOneShot(footstepClips[index]);
        }

        public void JumpButton()
        {
            if (characterController.isGrounded)
            {
                jumpRequest = true;
            }
        }
    }
}