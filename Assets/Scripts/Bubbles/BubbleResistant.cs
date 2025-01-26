using FMODUnity;
using UnityEngine;

public class BubbleResistant : Bubble
{
	[SerializeField]
	private uint _popsAmount = 2u;
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
		base.Push();
		Hit();
	}

	public override void Release()
	{
		base.Release();
		++_timesPopped;
		if (_timesPopped < _popsAmount)
		{
			_animator.CrossFade("BubbleResistantInversePush", 0.1f);
		}
	}

	protected void Hit()
	{
		if (PowerUpManager.Instance.IsPowerUpInUse("Chopstick"))
		{
			_timesPopped = _popsAmount + 2;
        }

        if (_timesPopped + 1 >= _popsAmount)
		{
			Pop();
		}
	}

	protected override void Pop()
	{
		_playOneShotAudio.Play(_resistantPopEvent);
		_animator.CrossFade("Push", 0.1f);
		base.Pop();
	}
}
