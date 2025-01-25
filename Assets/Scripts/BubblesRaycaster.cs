using UnityEngine;
using UnityEngine.InputSystem;

public class BubblesRaycaster : MonoBehaviour
{
	[SerializeField]
	private LayerMask _layerMask;

	private InputAction _pop = null;
	private Bubble _pushedBubble = null;

	private void Start()
	{
		PlayerInput playerInput = GetComponent<PlayerInput>();
		_pop = playerInput.currentActionMap.FindAction("BubblePop");
		_pop.performed += OnBubblePush;
	}

	private void Update()
	{
		if (_pushedBubble != null && _pop.WasReleasedThisFrame())
		{
			OnBubblePop();
		}
	}

	// Called from Input System
	public void OnBubblePush(InputAction.CallbackContext obj)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Collide))
		{
			Bubble bubble = hitInfo.collider.GetComponent<Bubble>();
			bubble.Push();
			_pushedBubble = bubble;
		}
	}

	private void OnBubblePop()
	{
		_pushedBubble.Pop();
		_pushedBubble = null;
	}
}
