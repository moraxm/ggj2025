using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate3DObject : MonoBehaviour
{
    #region Variables

    private bool _rotateAllowed = false;

    [SerializeField] private float _speed = 10.0f;

    [SerializeField] private bool _invertedX = false;
    [SerializeField] private bool _invertedY = false;

    [SerializeField] private bool _zoomEnabled = true;
    [SerializeField] private Vector2 _maxDistance = new Vector2(-5f, -10f);
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

    private Camera _camera = null;

    #endregion


    private void Start()
    {
        _camera = Camera.main;

		InitializeInputSystem();
	}

	private void OnDestroy()
	{
        TerminateInputSystem();
	}

	private void InitializeInputSystem()
    {
        InputManager.Instance.RegisterOnRotateStarted(OnRightClickPressed);
		InputManager.Instance.RegisterOnRotatePerformed(OnRightClickPressed);
		InputManager.Instance.RegisterOnRotateCancelled(OnRightClickReleased);
	}

    private void TerminateInputSystem()
    {
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            InputManager.Instance.UnregisterOnRotateStarted(OnRightClickPressed);
            InputManager.Instance.UnregisterOnRotatePerformed(OnRightClickPressed);
            InputManager.Instance.UnregisterOnRotateCancelled(OnRightClickReleased);
        }
	}

    protected virtual void OnRightClickPressed()
    {
        _rotateAllowed = true;
    }

    protected virtual void OnRightClickReleased()
    {
		_rotateAllowed = false;
	}

    protected virtual Vector2 GetMouseLookInput()
    {
        return InputManager.Instance.ReadDeltaInput();
    }

    protected virtual float GetZoom()
    {
        return InputManager.Instance.ReadZoom();
    }

    private void Update()
    {
        if (GameManager.Instance.IsPlaying)
        {
            ManageZoom();
            ManageRotation();
        }
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

        MouseDelta *= _speed * GetFrameDuration();

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
