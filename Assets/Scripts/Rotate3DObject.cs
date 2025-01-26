using Unity.Cinemachine;
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
    private float _lastDistanceTouch = 0;
    private bool _pitch2Press = false;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private float _timeToRotate = 0.2f;
    private float _timeAcumRotation = 0;

    #endregion
    private void Start()
    {
        _layerMask = LayerMask.GetMask("Bubble", "Object");

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

        InputManager.Instance.RegisterOnPitch2Started(OnPitch2Started);
        InputManager.Instance.RegisterOnPitch2Cancelled(OnPitch2Cancelled);
    }

    private void TerminateInputSystem()
    {
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            InputManager.Instance.UnregisterOnRotateStarted(OnRightClickPressed);
            InputManager.Instance.UnregisterOnRotatePerformed(OnRightClickPressed);
            InputManager.Instance.UnregisterOnRotateCancelled(OnRightClickReleased);

            InputManager.Instance.UnregisterOnPitch2Started(OnPitch2Started);
            InputManager.Instance.UnregisterOnPitch2Cancelled(OnPitch2Cancelled);
        }
	}

    protected virtual void OnPitch2Started()
    {
        _pitch2Press = true;
    }

    protected virtual void OnPitch2Cancelled()
    {
        _pitch2Press = false;
    }

    protected virtual void OnRightClickPressed()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.ReadCursorTouch());
        bool resultRaycast = Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Collide);
        if (ExpectedResultRaycast(resultRaycast))
        {
            _rotateAllowed = true;
        }
    }

    private bool ExpectedResultRaycast(bool result)
    {
        bool inUse = PowerUpManager.Instance.IsPowerUpInUse("DragSystem");
        if( inUse )
        {
            return !result;
        }
        else
        {
            return result;
        }

    }

    protected virtual void OnRightClickReleased()
    {
		_rotateAllowed = false;
        _timeAcumRotation = 0;
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

        ZoomMouse();
        ZoomTouch();
    }

    private void ZoomMouse()
    {
        float zoom = GetZoom();

        ApplyZoom(zoom);
    }

    private void ApplyZoom(float value)
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("CameraZoom");

        if(cameras.Length == 0)
        {
            Camera _camera = Camera.main;
            _camera.fieldOfView += -value * _factorZoom;
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, _maxDistance.x, _maxDistance.y);
        }
        else
        {
            for(int i = 0;i < cameras.Length; ++i)
            {
                CinemachineCamera cam = cameras[i].GetComponent<CinemachineCamera>();
                cam.Lens.FieldOfView += -value * _factorZoom;
                cam.Lens.FieldOfView = Mathf.Clamp(cam.Lens.FieldOfView, _maxDistance.x, _maxDistance.y);
            }
        }

        
    }

    private void ZoomTouch()
    {
        if (!_pitch2Press) { return; }

        Vector2 pitch1 = InputManager.Instance.ReadPitch1();
        Vector2 pitch2 = InputManager.Instance.ReadPitch2();

        float distance = Vector2.Distance(pitch1, pitch2);

        if(distance > _lastDistanceTouch)
        {
            ApplyZoom(1);
        }

        if (distance < _lastDistanceTouch)
        {
            ApplyZoom(-1);
        }

        _lastDistanceTouch = distance;
    }

    private void ManageRotation()
    {
        if (!_rotateAllowed)
        {
            return;
        }

        _timeAcumRotation += Time.deltaTime;

        if(_timeAcumRotation > _timeToRotate)
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

    float GetFrameDuration()
    {
        float t = Time.deltaTime;
        return t;
    }
}
