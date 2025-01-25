using UnityEngine;

public class BubbleResistant : Bubble
{
	[SerializeField]
	private uint _popsAmount = 2u;

	private uint _timesPopped = 0u;

	public override void Push()
	{
		_animator.CrossFade("BubbleResistantPush", 0.1f);
		// TODO: Sonido Semipush
	}

	public override void Pop()
	{
		++_timesPopped;
		if (_timesPopped >= _popsAmount)
		{
			ActualPop();
		}
		else
		{
			_animator.CrossFade("BubbleResistantInversePush", 0.1f);
			// TODO: Sonido volver de semipush a normal
		}
	}

	private void ActualPop()
	{
		base.Pop();
		// TODO: Sonido Pop
	}
}
