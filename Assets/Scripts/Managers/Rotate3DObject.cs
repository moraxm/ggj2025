using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate3DObject : MonoBehaviour
{
    #region Input Actions
    [SerializeField]
    private InputActionAsset _actions;

    public InputActionAsset actions
    {
        get => _actions;
        set => _actions = value;
    }

    protected InputAction leftClickPressedInputAction { get; set; }

    protected InputAction mouseLookInputAction { get; set; }

    #endregion

    #region Variables

    private bool _rotateAllowed;

    private Camera _camera;

    [SerializeField] private float _speed;

    [SerializeField] private bool _invertedX;
    [SerializeField] private bool _invertedY;

    #endregion

    private void Awake()
    {
        InitializeInputSystem();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void InitializeInputSystem()
    {
        leftClickPressedInputAction = actions.FindActionMap("DragObject").FindAction("Click");
        if (leftClickPressedInputAction != null)
        {
            leftClickPressedInputAction.started += OnRightClickPressed;
            leftClickPressedInputAction.performed += OnRightClickPressed;
            leftClickPressedInputAction.canceled += OnRightClickPressed;
        }

        mouseLookInputAction = actions.FindActionMap("DragObject").FindAction("Delta");

        actions.Enable();
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
        if (mouseLookInputAction != null)
            return mouseLookInputAction.ReadValue<Vector2>();

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
