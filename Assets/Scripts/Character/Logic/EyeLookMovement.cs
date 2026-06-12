using System.Collections;
using Lean.Gui;
using UnityEngine;

public class EyeLookMovement : MonoBehaviour
{
    [SerializeField] private LeanJoystick _joystick_horizontal;
    [SerializeField] private LeanJoystick _joystick_vertical;
    [SerializeField] private LeanJoystick _joystick;
    [SerializeField] private float duration;
    private UDPHandler _udp;
    float threshold = 0.5f;
    float hz = 0.5f;

    void Start()
    {
        _udp = FindAnyObjectByType<UDPHandler>();

        if (_udp == null)
        {
            Debug.LogError("[EyeLookMovement] UDPHandler not found.");
        }

        StartCoroutine(Handle());
    }

    void HandleJoysticks()
    {
        float horizontal = _joystick_horizontal.ScaledValue.y;
        float vertical = _joystick_vertical.ScaledValue.x;
        horizontal = Mathf.Abs(_joystick.ScaledValue.x) > Mathf.Abs(horizontal) && _joystick.ScaledValue.x * horizontal >= 0 ? _joystick.ScaledValue.x : horizontal;
        vertical = Mathf.Abs(_joystick.ScaledValue.y) > Mathf.Abs(vertical) && _joystick.ScaledValue.y * vertical >= 0 ? _joystick.ScaledValue.y : vertical;

        Debug.Log(horizontal + " " + vertical);
        /// direction: 0=right 1=left 2=down 3=up 4=right-down 5=left-down 6=right-up 7=left-up

        if (horizontal > threshold && vertical <= threshold && vertical > -threshold)
        {
            _udp.SendSetLookDirection(0, (byte)(duration * 10));
        }
        else if (horizontal < -threshold && vertical <= threshold && vertical > -threshold)
        {
            _udp.SendSetLookDirection(1, (byte)(duration * 10));
        }
        else if (vertical > threshold && horizontal <= threshold && horizontal > -threshold)
        {
            _udp.SendSetLookDirection(3, (byte)(duration * 10));
        }
        else if (vertical < -threshold && horizontal <= threshold && horizontal > -threshold)
        {
            _udp.SendSetLookDirection(2, (byte)(duration * 10));
        }
        else if (horizontal > threshold && vertical > threshold)
        {
            _udp.SendSetLookDirection(4, (byte)(duration * 10));
        }
        else if (horizontal < -threshold && vertical > threshold)
        {
            _udp.SendSetLookDirection(5, (byte)(duration * 10));
        }
        else if (horizontal > threshold && vertical < -threshold)
        {
            _udp.SendSetLookDirection(6, (byte)(duration * 10));
        }
        else if (horizontal < -threshold && vertical < -threshold)
        {
            _udp.SendSetLookDirection(7, (byte)(duration * 10));
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

}
