using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class MusicController : MonoBehaviour
{
	private const string kParameterName = "MusicParameter";

	[SerializeField]
    private EventReference _musicEvtRef;
	[SerializeField]
	private UnityEvent _onFinaleStart = null;

	private EventInstance _musicInstance;
	private float _parameterToApply = 0.0f;
	private int _lastPlaybackPosition = 0;
	private bool _readyForFinale = false;

    private void Start()
    {
        if (!_musicEvtRef.IsNull)
        {
			_musicInstance = AudioManager.Instance.CreateInstance(_musicEvtRef);
            _musicInstance.start();
        }
    }

	private void Update()
	{
		int pos = AudioManager.Instance.GetTimelinePosition(_musicInstance);
		// Music event loop restarted
		if (pos < _lastPlaybackPosition)
		{
			if (!_readyForFinale)
			{
				AudioManager.Instance.SetParameter(_musicInstance, kParameterName, _parameterToApply);
			}
            else
            {
				AudioManager.Instance.StopAudioEvent(_musicInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				_onFinaleStart?.Invoke();
			}
        }
		_lastPlaybackPosition = pos;
	}

	public void ChangeParameter(float newValue)
    {
		_parameterToApply = newValue;
	}

	public void SetReadyForFinale()
	{
		_readyForFinale = true;
	}

	private void OnDestroy()
	{
		if (_musicInstance.isValid())
        {
            _musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _musicInstance.release();
        }
	}
}
