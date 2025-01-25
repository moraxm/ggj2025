using UnityEngine;

public class BubbleLongPush : Bubble
{
	[SerializeField]
	private float _timeToPush = 1.0f;

	private bool _pushed = false;
	private float _timePushed = 0.0f;

	protected override void Update()
	{
		base.Update();

		if (_pushed)
		{
			_timePushed = Mathf.Min(_timePushed + Time.deltaTime, _timeToPush);
			if (_timePushed >= _timeToPush )
			{
				ActualPop();
			}
		}
	}

	public override void Push()
	{
		base.Push();
		_pushed = true;
		// TODO: Sonido Long Push
	}

	public override void Pop()
	{
		if (_pushed)
		{
			_animator.CrossFade("InversePush", 0.1f);
			_pushed = false;
			_timePushed = 0.0f;
		}
	}

	private void ActualPop()
	{
		base.Pop();
		_pushed = false;
		// TODO: Sonido Long Push Pop
	}
}
