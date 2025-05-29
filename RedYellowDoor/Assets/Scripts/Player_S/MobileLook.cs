using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileLook : MonoBehaviour
{
    public Camera playerCamera;         // Assign your camera
    public float lookSpeed = 0.1f;      // Lowered because touch delta is large
    public float lookXLimit = 45.0f;

    private float rotationX = 0f;
    private bool dragging = false;

    void Start()
    {
        rotationX = playerCamera.transform.localEulerAngles.x;
        if (rotationX > 180f) rotationX -= 360f;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x < Screen.width / 2f)
                return;

            if (touch.phase == TouchPhase.Began)
            {
                dragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && dragging)
            {
                Vector2 delta = touch.deltaPosition;

                // Horizontal (Y axis): rotate the player
                transform.Rotate(0, delta.x * lookSpeed, 0);

                // Vertical (X axis): rotate only the camera
                rotationX -= delta.y * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localEulerAngles = new Vector3(rotationX, 0f, 0f);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                dragging = false;
            }
        }
    }
}
