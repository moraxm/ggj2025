using FMODUnity;
using UnityEngine;

public class BubbleLongPush : Bubble
{
	[SerializeField]
	private float _timeToPush = 1.0f;
	[SerializeField]
	private EventReference _longPushEvent;
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

            if (canDoPop() )
			{
				ActualPop();
			}
		}
	}

	private bool canDoPop()
	{
        if (PowerUpManager.Instance.IsPowerUpInUse("Chopstick"))
		{
			return true;
		}	
        return _timePushed >= _timeToPush;
    }

	public override void Push()
	{
		_playOneShotAudio.Play(_longPushEvent);
		base.Push();
		_pushed = true;
	}

	public override void Pop()
	{
		if (_pushed)
		{
			_animator.CrossFade("InversePush", 0.1f);
			_pushed = false;
			_timePushed = 0.0f;
		}
		else
		{
			// TODO: Sonido volver de push a normal
		}
	}

	private void ActualPop()
	{
		_playOneShotAudio.Play(_longPopEvent);
		base.Pop();
		_pushed = false;
	}
}
