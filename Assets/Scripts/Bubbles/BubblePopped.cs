public class BubblePopped : Bubble
{
	protected override void Start()
	{
		_animator.CrossFade("BubblePopped", 0.0f);
		if (_collider != null)
		{
			_collider.enabled = false;
		}
	}

	public override void Push()
	{
		// Do nothing (collider is disabled)
	}

	public override void Release()
	{
		// Do nothing (collider is disabled)
	}

	public override void Pop()
	{
		// Do nothing (collider is disabled)
	}
}
