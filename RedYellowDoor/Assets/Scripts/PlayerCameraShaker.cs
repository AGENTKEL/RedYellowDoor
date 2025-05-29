using UnityEngine;

public class PlayerCameraShaker : MonoBehaviour
{
    private Vector3 originalPosition;
    [SerializeField] private float shakeDuration = 0f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float dampingSpeed = 1.0f;

    private bool shaking = false;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
            shaking = true;
        }
        else if (shaking)
        {
            shakeDuration = 0f;
            transform.localPosition = originalPosition;
            shaking = false;
        }
    }

    /// <summary>
    /// Call this method to start a screen shake.
    /// </summary>
    /// <param name="duration">How long to shake (in seconds).</param>
    /// <param name="magnitude">How intense the shake is.</param>
    /// <param name="damping">How fast it fades out (higher = faster).</param>
    public void StartShake(float duration, float magnitude = 0.1f, float damping = 1f)
    {
        originalPosition = transform.localPosition;
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        dampingSpeed = damping;
        shaking = true;
    }
}
