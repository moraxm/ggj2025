using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	private static InputManager _instance = null;

	private PlayerInput _playerInput = null;
	private InputAction _bubblePopAction = null;
	private InputAction _deltaInputAction = null;
	private InputAction _zoomInputAction = null;
	private InputAction _cursorTouch = null;
    private InputAction _pitch1Position = null;
    private InputAction _pitch2Position = null;

    private Action _onRotateStarted = null;
	private Action _onRotatePerformed = null;
	private Action _onRotateCancelled = null;
	private Action _onBubblePopPerformed = null;
	private Action _onBackPerformed = null;

    private Action _onPitch2Started = null;
    private Action _onPitch2Cancelled = null;

    public static InputManager Instance => _instance;

	public void RegisterOnRotateStarted(Action onRotateStarted)
	{
		_onRotateStarted += onRotateStarted;
	}
	public void RegisterOnRotatePerformed(Action onRotatePerformed)
	{
		_onRotatePerformed += onRotatePerformed;
	}
	public void RegisterOnRotateCancelled(Action onRotateCancelled)
	{
		_onRotateCancelled += onRotateCancelled;
	}
	public void RegisterOnBubblePopPerformed(Action onBubblePopPerformed)
	{
		_onBubblePopPerformed += onBubblePopPerformed;
	}
	public void RegisterOnBackPerformed(Action onBackPerformed)
	{
		_onBackPerformed += onBackPerformed;
	}

    public void RegisterOnPitch2Started(Action callback)
    {
        _onPitch2Started += callback;
    }
    public void RegisterOnPitch2Cancelled(Action callback)
    {
        _onPitch2Cancelled += callback;
    }

    public void UnregisterOnRotateStarted(Action onRotateStarted)
	{
		_onRotateStarted -= onRotateStarted;
	}
	public void UnregisterOnRotatePerformed(Action onRotatePerformed)
	{
		_onRotatePerformed -= onRotatePerformed;
	}
	public void UnregisterOnRotateCancelled(Action onRotateCancelled)
	{
		_onRotateCancelled -= onRotateCancelled;
	}
	public void UnregisterOnBubblePopPerformed(Action onBubblePopPerformed)
	{
		_onBubblePopPerformed -= onBubblePopPerformed;
	}
	public void UnregisterOnBackPerformed(Action onBackPerformed)
	{
		_onBackPerformed -= onBackPerformed;
	}

    public void UnregisterOnPitch2Started(Action callback)
    {
        _onPitch2Started -= callback;
    }

    public void UnregisterOnPitch2Cancelled(Action callback)
    {
        _onPitch2Cancelled -= callback;
    }

    private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
			Init();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			Terminate();
		}
	}

	private void Init()
	{
		_playerInput = FindFirstObjectByType<PlayerInput>();

		if (_playerInput != null)
		{
			InputAction rotateInputAction = _playerInput.currentActionMap.FindAction("RotateClick");
			if (rotateInputAction != null)
			{
				rotateInputAction.started += OnRotateStarted;
				rotateInputAction.performed += OnRotatePerformed;
				rotateInputAction.canceled += OnRotateCancelled;
			}
			_bubblePopAction = _playerInput.currentActionMap.FindAction("BubblePop");
			if (_bubblePopAction != null)
			{
				_bubblePopAction.performed += OnBubblePopPerformed;
			}
			InputAction backAction = _playerInput.currentActionMap.FindAction("Back");
			if (backAction != null)
			{
				backAction.performed += OnBackperformed;
			}

			_deltaInputAction = _playerInput.currentActionMap.FindAction("Delta");
			_zoomInputAction = _playerInput.currentActionMap.FindAction("Zoom");
            _cursorTouch = _playerInput.currentActionMap.FindAction("CursorTouch");
            _pitch1Position = _playerInput.currentActionMap.FindAction("Pitch1Position");
            _pitch2Position = _playerInput.currentActionMap.FindAction("Pitch2Position");

            InputAction _pitch2Press = _playerInput.currentActionMap.FindAction("Pitch2Press");
            if (_pitch2Press != null)
            {
                _pitch2Press.started += OnPitch2Started;
                _pitch2Press.canceled += OnPitch2Cancelled;
            }
        }
    }

	private void Terminate()
	{
		if (_playerInput != null)
		{
			InputActionMap actionMap = _playerInput.currentActionMap;
			if (actionMap != null)
			{
				InputAction rotateInputAction = actionMap.FindAction("RotateClick");
				if (rotateInputAction != null)
				{
					rotateInputAction.started -= OnRotateStarted;
					rotateInputAction.performed -= OnRotatePerformed;
					rotateInputAction.canceled -= OnRotateCancelled;
				}
				if (_bubblePopAction != null)
				{
					_bubblePopAction.performed -= OnBubblePopPerformed;
				}
				InputAction backAction = _playerInput.currentActionMap.FindAction("Back");
				if (backAction != null)
				{
					backAction.performed -= OnBackperformed;
				}
			}
		}
	}

	private void OnRotateStarted(InputAction.CallbackContext context)
	{
		_onRotateStarted?.Invoke();
	}

	private void OnRotatePerformed(InputAction.CallbackContext context)
	{
		_onRotatePerformed?.Invoke();
	}

	private void OnRotateCancelled(InputAction.CallbackContext context)
	{
		_onRotateCancelled?.Invoke();
	}

	private void OnBubblePopPerformed(InputAction.CallbackContext context)
	{
		_onBubblePopPerformed?.Invoke();
	}

	private void OnBackperformed(InputAction.CallbackContext context)
	{
		_onBackPerformed?.Invoke();
	}

	public bool BubblePopReleased()
	{
		if (_bubblePopAction != null)
		{
			return _bubblePopAction.WasReleasedThisFrame();
		}

		return false;
	}

	public Vector2 ReadDeltaInput()
	{
		if (_deltaInputAction != null)
		{
			return _deltaInputAction.ReadValue<Vector2>();
		}

		return Vector2.zero;
	}

	public float ReadZoom()
	{
		if (_zoomInputAction != null)
		{
			return _zoomInputAction.ReadValue<float>();
		}

		return 0.0f;
	}

    public Vector2 ReadCursorTouch()
    {
        if (_cursorTouch != null)
        {
            return _cursorTouch.ReadValue<Vector2>();
        }

        return Vector2.zero;
    }

    public Vector2 ReadPitch1()
    {
        if (_pitch1Position != null)
        {
            return _pitch1Position.ReadValue<Vector2>();
        }

        return Vector2.zero;
    }

    public Vector2 ReadPitch2()
    {
        if (_pitch2Position != null)
        {
            return _pitch2Position.ReadValue<Vector2>();
        }

        return Vector2.zero;
    }

    private void OnPitch2Started(InputAction.CallbackContext context)
    {
        _onPitch2Started?.Invoke();
    }

    private void OnPitch2Cancelled(InputAction.CallbackContext context)
    {
        _onPitch2Cancelled?.Invoke();
    }
}
