using UnityEngine;

public class BubbleResistant : Bubble
{
	[SerializeField]
	private uint _popsAmount = 2u;

	protected PlayOneShotAudio _playOneShotAudio = null;
	private uint _timesPopped = 0u;

	protected override void Awake()
	{
		base.Awake();
		_playOneShotAudio = GetComponent<PlayOneShotAudio>();
	}

	public override void Push()
	{
		// TODO: Sonido Semipush
		base.Push();
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
		// TODO: Sonido Pop
		base.Pop();
	}
}
