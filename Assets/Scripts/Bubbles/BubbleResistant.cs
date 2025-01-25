using FMODUnity;
using UnityEngine;

public class BubbleResistant : Bubble
{
	[SerializeField]
	private uint _popsAmount = 2u;
	[SerializeField]
	private EventReference _resistantPushEvent;
	[SerializeField]
	private EventReference _resistantPopEvent;

	protected PlayOneShotAudio _playOneShotAudio = null;
	private uint _timesPopped = 0u;

	protected override void Awake()
	{
		base.Awake();
		_playOneShotAudio = GetComponent<PlayOneShotAudio>();
	}

	public override void Push()
	{
		_playOneShotAudio.Play(_resistantPushEvent);
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
		}
	}

	private void ActualPop()
	{
		_playOneShotAudio.Play(_resistantPopEvent);
		base.Pop();
	}
}
