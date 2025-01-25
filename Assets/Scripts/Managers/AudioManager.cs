using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	#region Initialization
	private static AudioManager _instance = null;

	public static AudioManager Instance => _instance;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);

			LoadBank("Master", true);
			LoadBank("Master.strings", true);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			UnloadBank("Master.strings");
			UnloadBank("Master");
		}
	}
	#endregion

	#region Load Banks
	public void LoadBank(string bank, bool preloadSamples)
	{
		try
		{
			RuntimeManager.LoadBank(bank, preloadSamples);
		}
		catch (BankLoadException e)
		{
			RuntimeUtils.DebugLogException(e);
		}
		RuntimeManager.WaitForAllSampleLoading();
	}

	public void UnloadBank(string bank)
	{
		RuntimeManager.UnloadBank(bank);
	}
	#endregion

	#region Playing Events
	public void PlayOneShot(EventReference eventRef, Vector3 position = default)
	{
		RuntimeManager.PlayOneShot(eventRef, position);
	}

	public void PlayOneShot(string eventPath, Vector3 position = default)
	{
		RuntimeManager.PlayOneShot(eventPath, position);
	}

	public void PlayOneShotAttached(EventReference eventRef, GameObject go)
	{
		RuntimeManager.PlayOneShotAttached(eventRef, go);
	}

	public void PlayOneShotAttached(string eventPath, GameObject go)
	{
		RuntimeManager.PlayOneShotAttached(eventPath, go);
	}

	public EventInstance CreateInstance(EventReference eventRef, Vector3 position = default)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventRef);
		eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
		return eventInstance;
	}

	public EventInstance CreateInstance(string eventPath, Vector3 position = default)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventPath);
		eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
		return eventInstance;
	}

	public EventInstance CreateInstanceAttached(EventReference eventReference, GameObject go)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
#if UNITY_PHYSICS_EXIST
		RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject, gameObject.GetComponent<Rigidbody>());
#elif UNITY_PHYSICS2D_EXIST
		RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject, gameObject.GetComponent<Rigidbody2D>());
#else
		RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject);
#endif
		return eventInstance;
	}

	public EventInstance CreateInstanceAttached(string eventPath, GameObject go)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventPath);
#if UNITY_PHYSICS_EXIST
		RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject, gameObject.GetComponent<Rigidbody>());
#elif UNITY_PHYSICS2D_EXIST
		RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject, gameObject.GetComponent<Rigidbody2D>());
#else
		RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject);
#endif
		return eventInstance;
	}
	#endregion

	#region Stop
	public void StopAudioEvent(EventInstance eventInstance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (eventInstance.isValid())
		{
			eventInstance.stop(stopMode);
			eventInstance.release();
		}
	}
	#endregion

	#region Parameters
	public void SetParameter(EventInstance eventInstance, string parameterName, float parameterValue)
	{
		if (eventInstance.isValid())
		{
			eventInstance.setParameterByName(parameterName, parameterValue);
		}
	}
	#endregion
}
