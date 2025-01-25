using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PlayLoopOnStart : MonoBehaviour
{
    [SerializeField]
    private EventReference _evtRef;

    private EventInstance _loopInstance;

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
            ChangeParameter("MusicParameter", 0.0f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
			ChangeParameter("MusicParameter", 1.0f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ChangeParameter("MusicParameter", 2.0f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			ChangeParameter("MusicParameter", 3.0f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			ChangeParameter("MusicParameter", 4.0f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			ChangeParameter("MusicParameter", 5.0f);
		}
	}

	public void ChangeParameter(string parameterName, float newValue)
    {
        AudioManager.Instance.SetParameter(_loopInstance, parameterName, newValue);
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
