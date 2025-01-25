using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate3DObject : MonoBehaviour
{
    #region Input Actions

    protected InputAction _rotateInputAction { get; set; }

    protected InputAction _deltaInputAction { get; set; }

    protected InputAction _zoomInputAction { get; set; }

    #endregion

    #region Variables

    private bool _rotateAllowed = false;

    [SerializeField] private float _speed = 10.0f;

    [SerializeField] private bool _invertedX = false;
    [SerializeField] private bool _invertedY = false;

    [SerializeField] private bool _zoomEnabled = true;
    [SerializeField] private Vector2 _maxDistance = new Vector2(-5f, -10f);
    [SerializeField] private float _factorZoom = 2;

    private Camera _camera = null;
	private PlayerInput _playerInput = null;

	#endregion

    private void Start()
    {
        _camera = Camera.main;
        _playerInput = FindFirstObjectByType<PlayerInput>();

		InitializeInputSystem();
	}

    private void InitializeInputSystem()
    {
        _rotateInputAction = _playerInput.currentActionMap.FindAction("RotateClick");
        if (_rotateInputAction != null)
        {
            _rotateInputAction.started += OnRightClickPressed;
            _rotateInputAction.performed += OnRightClickPressed;
            _rotateInputAction.canceled += OnRightClickPressed;
        }

        _deltaInputAction = _playerInput.currentActionMap.FindAction("Delta");

        _zoomInputAction = _playerInput.currentActionMap.FindAction("Zoom");
    }

    protected virtual void OnRightClickPressed(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            _rotateAllowed = true;
        }
        else if (context.canceled)
        {
            _rotateAllowed = false;
        }
    }

    protected virtual Vector2 GetMouseLookInput()
    {
        if (_deltaInputAction != null)
        {
            return _deltaInputAction.ReadValue<Vector2>();
        }

        return Vector2.zero;
    }

    protected virtual float GetZoom()
    {
        if (_zoomInputAction != null)
        {
            return _zoomInputAction.ReadValue<float>();
        }

        return 0;
    }

    private void Update()
    {
        ManageZoom();
        ManageRotation();
    }

    private void ManageZoom()
    {
        if(!_zoomEnabled)
        {
            return;
        }

        float zoom = GetZoom();

        _camera.fieldOfView += -zoom * _factorZoom;

        _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, _maxDistance.x, _maxDistance.y);
    }

    private void ManageRotation()
    {
        if (!_rotateAllowed)
        {
            return;
        }

        Vector2 MouseDelta = GetMouseLookInput();

        MouseDelta *= _speed * Time.deltaTime;

        transform.Rotate(Vector3.up * (_invertedX ? 1 : -1), MouseDelta.x, Space.World);
        transform.Rotate(Vector3.right * (_invertedY ? -1 : 1), MouseDelta.y, Space.World);
    }
}
