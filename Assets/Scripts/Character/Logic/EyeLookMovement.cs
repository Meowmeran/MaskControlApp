using System.Collections;
using Lean.Gui;
using UnityEngine;

public class EyeLookMovement : MonoBehaviour
{
    [SerializeField] private LeanJoystick _joystick_horizontal;
    [SerializeField] private LeanJoystick _joystick_vertical;
    [SerializeField] private LeanJoystick _joystick;
    [SerializeField] private float duration = 1f;
    [SerializeField] private UDPHandler _udp;
    float threshold = 0.5f;
    float hz = 0.5f;

    void Start()
    {

        if (_udp == null)
        {
            _udp = FindAnyObjectByType<UDPHandler>();

            if (_udp == null)
            {
                Debug.LogWarning("[EyeLookMovement] UDP not ready.");
                return;
            }
        }

        StartCoroutine(Handle());
    }

    void HandleJoysticks()
    {
        if (_joystick.ScaledValue.x == 0 && _joystick.ScaledValue.y == 0 && _joystick_horizontal.ScaledValue.x == 0 && _joystick_horizontal.ScaledValue.y == 0 && _joystick_vertical.ScaledValue.x == 0 && _joystick_vertical.ScaledValue.y == 0)
        {
            return;
        }
        float horizontal = _joystick_horizontal.ScaledValue.x;
        float vertical = _joystick_vertical.ScaledValue.y;
        Debug.Log(horizontal + " " + vertical);
        horizontal = Mathf.Abs(_joystick.ScaledValue.x) > Mathf.Abs(horizontal) && _joystick.ScaledValue.x * horizontal >= 0 ? _joystick.ScaledValue.x : horizontal;
        vertical = Mathf.Abs(_joystick.ScaledValue.y) > Mathf.Abs(vertical) && _joystick.ScaledValue.y * vertical >= 0 ? _joystick.ScaledValue.y : vertical;


        int direction = DetectDirection(horizontal, vertical);
        Debug.Log(direction);
        if (direction != -1)
        {
            Debug.Log($"Sending direction {direction}");
            _udp.SendSetLookDirection((byte)direction, (byte)(duration * 10));
        }

    }

    IEnumerator Handle()
    {
        while (true)
        {
            HandleJoysticks();
            yield return new WaitForSeconds(hz);
        }
    }

    // direction: 0=right 1=left 2=down 3=up 4=right-down 5=left-down 6=right-up 7=left-up
    private int DetectDirection(float x, float y)
    {
        if (x > threshold && y > threshold) return 7;
        if (x < -threshold && y > threshold) return 6;
        if (x < -threshold && y < -threshold) return 4;
        if (x > threshold && y < -threshold) return 5;
        if (x > threshold && y > -threshold && y < threshold) return 1;
        if (x < -threshold && y > -threshold && y < threshold) return 0;
        if (y > threshold && x < threshold && x > -threshold) return 3;
        if (y < -threshold && x < threshold && x > -threshold) return 2;
        return -1;
    }

}
