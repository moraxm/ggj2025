using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Rotate3DObject : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput = null;

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
    [SerializeField] private Vector2 _maxDistance = new Vector2(-5, -10 );
    [SerializeField] private Camera _camera;
    [SerializeField] private float _factorZoom = 2;


    [SerializeField] private bool _useAceleration = false;
    [SerializeField] private float _weight = 40;
    [SerializeField] private float _accelerationBrakeMagnitude = -3;
    [SerializeField] private float _maxAceleration = 30;
    [SerializeField] private float _maxVelocity = 30;

    private Vector2 _velocity = Vector2.zero;
    private Vector2 _acceleration = Vector2.zero;
    private Vector2 _brakeVector;
    private Vector2 _originalSignVelocity;

    #endregion

    private void Awake()
    {
        InitializeInputSystem();
    }

    private void Start()
    {
        if(_camera == null)
        {
            _camera = Camera.main;
        }
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
            _rotateAllowed = false;

    }

    protected virtual Vector2 GetMouseLookInput()
    {
        if (_deltaInputAction != null)
            return _deltaInputAction.ReadValue<Vector2>();

        return Vector2.zero;
    }

    protected virtual float GetZoom()
    {
        if (_zoomInputAction != null)
            return _zoomInputAction.ReadValue<float>();

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
        if (_useAceleration)
        {
            AddNegativeAceleration();
            UpdateRotationWithAceleration();
        }


        if (!_rotateAllowed)
        {
            return;
        }

        if (_useAceleration)
        {
            ReadAceleration();
        }
        else
        {
            RotationLineal();
        }
        
    }

    private void RotationLineal()
    {
        Vector2 MouseDelta = GetMouseLookInput();

        MouseDelta *= _speed * Time.deltaTime;

        transform.Rotate(Vector3.up * (_invertedX ? 1 : -1), MouseDelta.x, Space.World);
        transform.Rotate(Vector3.right * (_invertedY ? -1 : 1), MouseDelta.y, Space.World);
    }

    private void ReadAceleration()
    {
        Vector2 MouseDelta = GetMouseLookInput();

        //f = m * a
        _acceleration += MouseDelta / _weight;
        _acceleration = Vector2.ClampMagnitude(_acceleration, _maxAceleration);
        _brakeVector = _acceleration.normalized * -1 * _accelerationBrakeMagnitude;
        _originalSignVelocity = CalculateSign(_acceleration);
    }

    private void AddNegativeAceleration()
    {
        if (!IsVelocityZero())
        {
            _acceleration += _brakeVector * GetFrameDuration();
        }
    }

    void UpdateRotationWithAceleration()
    {
        _velocity += _acceleration * GetFrameDuration();
        _velocity = Vector2.ClampMagnitude(_velocity, _maxVelocity);

        Vector2 currentSign =CalculateSign(_velocity);

        if (!IsVelocityZero() && currentSign == _originalSignVelocity)
        {
            transform.Rotate(Vector3.right * (_invertedY ? -1 : 1), _velocity.y, Space.World);
            transform.Rotate(Vector3.up * (_invertedX ? 1 : -1), _velocity.x, Space.World);
        }
        else
        {
            _velocity = Vector3.zero;
            _acceleration = Vector3.zero;
        }
    }

    bool IsVelocityZero()
    {
        return _velocity.sqrMagnitude < 0.001f;
    }

    float GetFrameDuration()
    {
        float t = Time.deltaTime;
        return t;
    }

    Vector2 CalculateSign(Vector2 reference)
    {
        int signX = reference.x < 0 ? -1 : 1;
        int signy = reference.y < 0 ? -1 : 1;
        return new Vector2(signX, signy);
    }
}
