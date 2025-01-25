using UnityEngine;
using UnityEngine.InputSystem;

public class BubblesRaycaster : MonoBehaviour
{
	[SerializeField]
	private LayerMask _layerMask;

	private Bubble _pushedBubble = null;

	private void Start()
	{
		InputManager.Instance.RegisterOnBubblePopPerformed(OnBubblePush);
	}

	private void OnDestroy()
	{
		InputManager inputManager = InputManager.Instance;
		if (inputManager != null)
		{
			InputManager.Instance.UnregisterOnBubblePopPerformed(OnBubblePush);
		}
	}

	private void Update()
	{
		if (_pushedBubble != null && InputManager.Instance.BubblePopReleased())
		{
			OnBubblePop();
		}
	}

	private void OnBubblePush()
	{
		Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.ReadCursorTouch());
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
