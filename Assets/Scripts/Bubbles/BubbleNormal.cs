using FMODUnity;
using UnityEngine;

public class BubbleNormal : Bubble
{
	[SerializeField]
	private EventReference _normalPopEvent;

	protected PlayOneShotAudio _playOneShotAudio = null;

	protected override void Awake()
	{
		base.Awake();
		_playOneShotAudio = GetComponent<PlayOneShotAudio>();
	}

	public override void Push()
	{
		base.Push();
		Pop();
	}

	public override void Release()
	{
		base.Release();
	}

	protected override void Pop()
	{
		_playOneShotAudio.Play(_normalPopEvent);
		base.Pop();
	}
}
