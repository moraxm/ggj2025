using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate3DObject : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput = null;

    #region Input Actions

    protected InputAction _leftClickPressedInputAction { get; set; }

    protected InputAction _mouseLookInputAction { get; set; }

    #endregion

    #region Variables

    private bool _rotateAllowed = false;

    [SerializeField] private float _speed = 10.0f;

    [SerializeField] private bool _invertedX = false;
    [SerializeField] private bool _invertedY = false;

    #endregion

    private void Awake()
    {
        InitializeInputSystem();
    }

    private void InitializeInputSystem()
    {
		_leftClickPressedInputAction = _playerInput.currentActionMap.FindAction("RotateClick");
        if (_leftClickPressedInputAction != null)
        {
            _leftClickPressedInputAction.started += OnRightClickPressed;
            _leftClickPressedInputAction.performed += OnRightClickPressed;
            _leftClickPressedInputAction.canceled += OnRightClickPressed;
        }

        _mouseLookInputAction = _playerInput.currentActionMap.FindAction("Delta");
    }

    protected virtual void OnRightClickPressed(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            _rotateAllowed = true;
        }
        else if (context.canceled)
            _rotateAllowed = false;

    }

    protected virtual Vector2 GetMouseLookInput()
    {
        if (_mouseLookInputAction != null)
            return _mouseLookInputAction.ReadValue<Vector2>();

        return Vector2.zero;
    }

    private void Update()
    {
        if (!_rotateAllowed)
            return;

        Vector2 MouseDelta = GetMouseLookInput();

        MouseDelta *= _speed * Time.deltaTime;

        transform.Rotate(Vector3.up * (_invertedX ? 1 : -1), MouseDelta.x, Space.World);
        transform.Rotate(Vector3.right * (_invertedY ? -1 : 1), MouseDelta.y, Space.World);
    }
}
