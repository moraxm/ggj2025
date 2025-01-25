using FMODUnity;
using UnityEngine;

public class PlayOneShotAudio : MonoBehaviour
{
	public void Play(EventReference eventRef)
	{
		if (!eventRef.IsNull)
		{
			AudioManager.Instance.PlayOneShot(eventRef, transform.position);
		}
	}
}
