using UnityEngine;
using UnityEngine.InputSystem;

public class BubblesRaycaster : MonoBehaviour
{
	[SerializeField]
	private LayerMask _layerMask;

	private void Start()
	{
		PlayerInput playerInput = GetComponent<PlayerInput>();
		InputAction pop = playerInput.currentActionMap.FindAction("BubblePop");
		pop.performed += OnBubblePop;
	}

	// Called from Input System
	public void OnBubblePop(InputAction.CallbackContext obj)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Collide))
		{
			Destroy(hitInfo.collider.gameObject);
		}
	}
}
