using UnityEngine;

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
		if (GameManager.Instance.IsPlaying &&
			_pushedBubble != null &&
			InputManager.Instance.BubblePopReleased())
		{
			OnBubbleRelease();
		}
	}

	private void OnBubblePush()
	{
		if (GameManager.Instance.IsPlaying)
		{
			Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.ReadCursorTouch());
			if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Collide))
			{
				Bubble bubble = hitInfo.collider.GetComponent<Bubble>();
				// We can also collide with the object itself
				if (bubble != null)
				{
					bubble.Push();
					_pushedBubble = bubble;
				}
			}
		}
	}

	private void OnBubbleRelease()
	{
		_pushedBubble.Release();
		_pushedBubble = null;
	}
}
