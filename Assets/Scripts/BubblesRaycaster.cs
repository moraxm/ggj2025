using System.Collections.Generic;
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
		if(GameManager.Instance.IsPlaying)
		{
			if(ManagePowerUpDrag())
			{
				return;
			}

            if (_pushedBubble != null &&
				InputManager.Instance.BubblePopReleased())
            {
                OnBubbleRelease();
            }
        }

		
	}

	bool ManagePowerUpDrag()
	{
        bool inUse = PowerUpManager.Instance.IsPowerUpInUse("DragSystem");
		if (!inUse) { return false; }

		OnBubblePush();
		if(_pushedBubble != null)
		{
            OnBubbleRelease();
			return true;
        }

		return false;
    }


	private bool CanPopBubble()
	{
		return false;
	}

	private void OnBubblePush()
	{
		if (GameManager.Instance.IsPlaying)
		{
			if(ManagePowerUpOneShot())
			{
				return;
			}

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

                if (PowerUpManager.Instance.IsPowerUpInUse("SphereCast"))
                {
                    ManageSphereCast(ray, hitInfo);
                }
            }
        }
	}

	private bool ManagePowerUpOneShot()
	{
        if (PowerUpManager.Instance.IsPowerUpInUse("LastOne"))
		{
			List<Bubble> bubbles = GameManager.Instance.GetCurrentObjectBubbles();

			for(int i = 0; i < bubbles.Count; ++i)
			{
				if (bubbles[i].GetComponent<Collider>().enabled)
				{
					bubbles[i].Pop();
					return true;
                }
			}

        }
        return false;
	}


    private void ManageSphereCast(Ray ray, RaycastHit hitInfo)
	{
		Vector3 originRay = ray.origin;
		Vector3 destinyPos = hitInfo.point;

		Vector3 half = (destinyPos + originRay) / 2;
		float dist = Vector3.Distance(half, originRay);

        Collider[] hitColliders = Physics.OverlapSphere(half, dist * 1.2f, _layerMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitColliders.Length; ++i)
		{
            Bubble bubble = hitColliders[i].GetComponent<Bubble>();
            // We can also collide with the object itself
            if (bubble != null)
            {
                bubble.Push();
                _pushedBubble = bubble;
            }
        }
    }



    private void OnBubbleRelease()
	{
		_pushedBubble.Release();
		_pushedBubble = null;
	}
}
