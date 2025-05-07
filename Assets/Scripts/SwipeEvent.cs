using System;
using UnityEngine;

public class SwipeInputController : MonoBehaviour
{
    public event Action<Vector2> OnSwipeUpdate;
    public event Action<Vector2> OnSwipeBegin;
    public event Action<Vector2> OnSwipeEnd;

    private Vector2 _swipeStartPosition;
    private Vector2 _currentSwipePosition;
    private bool _isSwipeActive = false;
    private int _activeTouchId = -1;

    private Action _processInput;

    private void Start()
    {
#if UNITY_EDITOR
        _processInput = ProcessMouseInput;
#elif UNITY_ANDROID
        inputAction = ProcessTouchInput;
#endif
    }

    private void Update() => _processInput?.Invoke();

    private void ProcessTouchInput()
    {
        if (Input.touchCount == 0)
            return;

        if (!_isSwipeActive)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    _activeTouchId = touch.fingerId;
                    StartSwipe(touch.position);
                    break;
                }
            }
        }
        else
        {
            bool touchFound = false;
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.fingerId == _activeTouchId)
                {
                    touchFound = true;
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        UpdateSwipe(touch.position);
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        EndSwipe();
                    }

                    break;
                }
            }

            if (!touchFound)
            {
                EndSwipe();
            }
        }
    }

    private void ProcessMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSwipe(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && _isSwipeActive)
        {
            UpdateSwipe(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && _isSwipeActive)
        {
            EndSwipe();
        }
    }

    private void StartSwipe(Vector2 position)
    {
        _swipeStartPosition = position;
        _isSwipeActive = true;
        _currentSwipePosition = position;
        OnSwipeBegin?.Invoke(_currentSwipePosition);
    }

    private void UpdateSwipe(Vector2 position)
    {
        _currentSwipePosition = position;
        OnSwipeUpdate?.Invoke(position);
    }

    private void EndSwipe()
    {
        _isSwipeActive = false;
        OnSwipeEnd?.Invoke(_currentSwipePosition);
    }
}