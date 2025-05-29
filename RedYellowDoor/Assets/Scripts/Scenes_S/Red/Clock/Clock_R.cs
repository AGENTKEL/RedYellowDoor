using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_R : MonoBehaviour
{
    public int minutes = 0;
    public int hour = 0;
    public int seconds = 0;
    public bool realTime = true;

    public GameObject pointerSeconds;
    public GameObject pointerMinutes;
    public GameObject pointerHours;

    public float clockSpeed = 1.0f; // 1.0f = realtime
    private float msecs = 0;

    public bool clockRunning = false; // <-- NEW
    public bool canKill = false;

    private float elapsedTime = 0f; // <-- NEW

    [SerializeField] private ClockManager clockManager;
    public float deathTime = 10f;
    

    void Start()
    {
        clockRunning = true;
        if (realTime)
        {
            hour = System.DateTime.Now.Hour;
            minutes = System.DateTime.Now.Minute;
            seconds = System.DateTime.Now.Second;
        }
    }

    void Update()
    {
        if (!clockRunning)
            return;

        msecs += Time.deltaTime * clockSpeed;
        elapsedTime += Time.deltaTime * clockSpeed;

        if (msecs >= 1.0f)
        {
            msecs -= 1.0f;
            seconds++;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                if (minutes > 60)
                {
                    minutes = 0;
                    hour++;
                    if (hour >= 24)
                        hour = 0;
                }
            }
        }

        float rotationSeconds = (360.0f / 60.0f) * seconds;
        float rotationMinutes = (360.0f / 60.0f) * minutes;
        float rotationHours = ((360.0f / 12.0f) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);

        pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);
        pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);
        pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationHours);

        // Kill player if 1 minute passes
        if (canKill)
        {
            if (elapsedTime >= deathTime)
            {
                clockManager.TriggerDeath();
                clockRunning = false; // stop clock
            }
        }
    }
}
