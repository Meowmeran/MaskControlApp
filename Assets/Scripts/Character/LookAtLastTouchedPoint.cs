using System.Collections;
using UnityEngine;

public class LookAtLastTouchedPoint : MonoBehaviour
{
    public bool isEnabled = true;
    [SerializeField] private GameObject target;
    [SerializeField] private ScreenInputPosition screenInputPosition;

    [Header("Look At Points")]
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;

    [Header("Timing")]
    [SerializeField] private float lerpSpeed = 5f; // Increased default for snappier responsiveness
    [SerializeField] private float maxLookDuration = 2f; // How long it's allowed to look while held

    private Quaternion _originalRotation;
    private Coroutine _lookCoroutine;

    private void Start()
    {
        if (target == null) target = gameObject;
        _originalRotation = target.transform.rotation;
        
        _lookCoroutine = StartCoroutine(LookAtRoutine());
    }

    /// <summary>
    /// Maps normalized screen position (0–1) onto the axis between point1 and point2.
    /// </summary>
    private Vector3 CalculateTargetPoint()
    {
        Vector2 screenPos = screenInputPosition.GetLastPosition();

        // Normalize to 0–1 range based on screen dimensions
        float x = Mathf.Clamp01(screenPos.x / Screen.width);
        float y = Mathf.Clamp01(screenPos.y / Screen.height);

        Vector3 delta = point2.position - point1.position;

        return new Vector3(
            point1.position.x + x * delta.x,
            point1.position.y + y * delta.y,
            point1.position.z // Keeps Z anchored to point1's plane
        );
    }

    private IEnumerator LookAtRoutine()
    {
        float lookTimer = 0f;

        while (true)
        {
            bool isTouching = screenInputPosition.IsTouching();

            if (isTouching && isEnabled)
            {
                // Increment timer while holding down touch
                lookTimer += Time.deltaTime;

                if (lookTimer < maxLookDuration)
                {
                    // 1. Calculate direction to target point
                    Vector3 direction = CalculateTargetPoint() - target.transform.position;
                    
                    if (direction != Vector3.zero)
                    {
                        // 2. Determine target look rotation
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        
                        // 3. Smoothly rotate towards it
                        target.transform.rotation = Quaternion.Slerp(
                            target.transform.rotation, 
                            targetRotation, 
                            Time.deltaTime * lerpSpeed
                        );
                    }
                }
                else
                {
                    // Exceeded max look duration; smoothly return home even if still touching
                    ReturnToOriginalRotation();
                }
            }
            else
            {
                // No touch: reset timer and smoothly return home
                lookTimer = 0f;
                ReturnToOriginalRotation();
            }

            yield return null;
        }
    }

    private void ReturnToOriginalRotation()
    {
        target.transform.rotation = Quaternion.Slerp(
            target.transform.rotation, 
            _originalRotation, 
            Time.deltaTime * lerpSpeed
        );
    }
}