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
		PlayMusic();
    }

	public void PlayMusic()
	{
		_parameterToApply = 0.0f;
		if (!_musicEvtRef.IsNull)
		{
			_musicInstance = AudioManager.Instance.CreateInstance(_musicEvtRef);
			_musicInstance.start();
			_parameterToApply = 1.0f;
			AudioManager.Instance.SetParameter(_musicInstance, kParameterName, _parameterToApply);
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
				//AudioManager.Instance.StopAudioEvent(_musicInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				//_onFinaleStart?.Invoke();
				//_readyForFinale = false;
			}
        }
		_lastPlaybackPosition = pos;
	}

	public void SetParameterForLevel(uint level)
	{
		switch (level)
		{
			case 1u:
				_parameterToApply = 3.0f;
				break;
			default:
				++_parameterToApply;
				break;
		}
	}

	public void SetReadyForFinale()
	{
        //_readyForFinale = true;

        AudioManager.Instance.StopAudioEvent(_musicInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _onFinaleStart?.Invoke();
        _readyForFinale = false;
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
