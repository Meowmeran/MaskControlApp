using UnityEngine;

public class ScreenInputPosition : MonoBehaviour
{
    private bool _isTouching;
    private Vector2 _lastPosition = Vector2.zero;
    private float _timeTouched;

    private void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            if (!_isTouching)
                _timeTouched = 0f;

            _isTouching = true;
            _timeTouched += Time.deltaTime;
            _lastPosition = Input.GetTouch(0).position;
        }
        else
        {
            _isTouching = false;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (!_isTouching)
                _timeTouched = 0f;

            _isTouching = true;
            _timeTouched += Time.deltaTime;
            _lastPosition = Input.mousePosition;
        }
        else
        {
            _isTouching = false;
        }
    }

    public Vector2 GetLastPosition() => _lastPosition;
    public bool IsTouching() => _isTouching;
    public float GetTime() => _timeTouched;
}