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

	private Action _onRotateStarted = null;
	private Action _onRotatePerformed = null;
	private Action _onRotateCancelled = null;
	private Action _onBubblePopPerformed = null;

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

			_deltaInputAction = _playerInput.currentActionMap.FindAction("Delta");
			_zoomInputAction = _playerInput.currentActionMap.FindAction("Zoom");
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
				InputAction bubblePopInputAction = actionMap.FindAction("BubblePop");
				if (bubblePopInputAction != null)
				{
					bubblePopInputAction.performed -= OnBubblePopPerformed;
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
}
