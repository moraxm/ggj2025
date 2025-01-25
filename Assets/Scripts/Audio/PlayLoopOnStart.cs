using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	private const string kParameterName = "MusicParameter";

	[SerializeField]
    private EventReference _evtRef;

    private EventInstance _loopInstance;
	private float _parameterToApply = 0.0f;
	private int _lastPlaybackPosition = 0;

    private void Start()
    {
        if (!_evtRef.IsNull)
        {
			_loopInstance = AudioManager.Instance.CreateInstance(_evtRef);
            _loopInstance.start();
        }
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeParameter(0.0f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
			ChangeParameter(1.0f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ChangeParameter(2.0f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			ChangeParameter(3.0f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			ChangeParameter(4.0f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			ChangeParameter(5.0f);
		}

		int pos = AudioManager.Instance.GetTimelinePosition(_loopInstance);
		// Music event loop restarted
		if (pos < _lastPlaybackPosition)
		{
			AudioManager.Instance.SetParameter(_loopInstance, kParameterName, _parameterToApply);
		}
		_lastPlaybackPosition = pos;
	}

	public void ChangeParameter(float newValue)
    {
		_parameterToApply = newValue;
	}

	private void OnDestroy()
	{
		if (_loopInstance.isValid())
        {
            _loopInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _loopInstance.release();
        }
	}
}
