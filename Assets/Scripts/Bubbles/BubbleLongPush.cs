using FMODUnity;
using UnityEngine;

public class BubbleLongPush : Bubble
{
	[SerializeField]
	private float _timeToPush = 1.0f;
	[SerializeField]
	private EventReference _longPopEvent;

	protected PlayOneShotAudio _playOneShotAudio = null;
	private bool _pushed = false;
	private float _timePushed = 0.0f;

	protected override void Awake()
	{
		base.Awake();
		_playOneShotAudio = GetComponent<PlayOneShotAudio>();
	}

	protected override void Update()
	{
		base.Update();

		if (_pushed)
		{
			_timePushed = Mathf.Min(_timePushed + Time.deltaTime, _timeToPush);
			if (_timePushed >= _timeToPush )
			{
				Pop();
			}
		}
	}

	public override void Push()
	{
		base.Push();
		_pushed = true;
	}

	public override void Release()
	{
		if (_pushed)
		{
			_animator.CrossFade("InversePush", 0.1f);
			_pushed = false;
			_timePushed = 0.0f;
		}
	}

	protected override void Pop()
	{
		_playOneShotAudio.Play(_longPopEvent);
		_animator.CrossFade("Pop", 0.1f);
		base.Pop();
		_pushed = false;
	}
}
