using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
	[System.Serializable]
	private struct LevelInfo
	{
		public BurbujasCreator ObjectPrefab;
		public float RoundTime;
	}

	private static GameManager _instance = null;

	public static GameManager Instance => _instance;

	[SerializeField]
	private MusicController _musicController = null;
	[SerializeField]
	private GameObject _gameHUD = null;
	[SerializeField]
	private Transform _objectsInitialTransform = null;
	[SerializeField]
	private Transform _objectsPlayTransform = null;
	[SerializeField]
	private TextMeshProUGUI _timeRemainingText = null;
	[SerializeField]
	private EventReference _wrapEvtRef;
	[SerializeField]
	private AutoKill _smoke = null;
	[SerializeField]
	private TextMeshProUGUI _bubblesRemainingText = null;
	[SerializeField]
	private float _timeToMoveObjectToPlayPos = 1.0f;
	[SerializeField]
	private float _timeToMoveObjectBackToInitPos = 1.0f;
	[SerializeField]
	private LevelInfo[] _levelsInfo = null;
    [SerializeField]
    private PlayableDirector _restartCinemachine = null;

    private BurbujasCreator[] _roundsObjects = null;
	private uint _currentLevel = 0u;
	private float _roundTime = 0.0f;
	private bool _playing = false;
	private uint _bubblesRemaining = 0u;

	public bool IsPlaying => _playing;

	private float RoundTime
	{
		get
		{
			return _roundTime;
		}
		set
		{
			_roundTime = value;
			_timeRemainingText.text = ((uint)value).ToString();
		}
	}

	private uint BubblesRemaining
	{
		get
		{
			return _bubblesRemaining;
		}
		set
		{
			_bubblesRemaining = value;
			_bubblesRemainingText.text = value.ToString();
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
			Init();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Init()
	{
		_roundsObjects = new BurbujasCreator[_levelsInfo.Length];
		for (int i = 0; i < _levelsInfo.Length; ++i)
		{
			BurbujasCreator roundObject = Instantiate(_levelsInfo[i].ObjectPrefab, _objectsInitialTransform.position, Quaternion.identity);
			if (roundObject != null)
			{
				_roundsObjects[i] = roundObject;
			}
		}
    }

	public void StartGame()
	{
		_gameHUD.SetActive(true);
		_musicController.SetParameterForLevel(_currentLevel);
		StartLevel(0u);
	}

	private void Update()
	{
		if (_playing)
		{
			RoundTime += Time.deltaTime;
#if DEBUG
			if (Input.GetKeyDown(KeyCode.Comma))
			{
				RoundCompleted();
			}
#endif
		}
	}

	private void StartLevel(uint level)
	{
		IEnumerator StartLevelCoroutine()
		{
			Debug.Log("Creating round " + level);
			EventInstance wrapEvent = AudioManager.Instance.CreateInstance(_wrapEvtRef);
			wrapEvent.start();
			yield return new WaitForSeconds(4.0f);
			wrapEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			wrapEvent.release();
			Transform objectTransform = _roundsObjects[_currentLevel].transform;
			float timeElapsed = 0.0f;
			while (timeElapsed < _timeToMoveObjectToPlayPos)
			{
				timeElapsed = Mathf.Min(timeElapsed + Time.deltaTime, _timeToMoveObjectToPlayPos);
				objectTransform.position = Vector3.Lerp(_objectsInitialTransform.position, _objectsPlayTransform.position, timeElapsed / _timeToMoveObjectToPlayPos); 
				yield return null;
			}
			BubblesRemaining = _roundsObjects[level].TotalBubbles;
			yield return new WaitForSeconds(0.2f);
			Debug.Log("Round started");
			// Enable rotation
			objectTransform.GetComponent<Rotate3DObject>().enabled = true;
			_playing = true;
			PowerUpManager.Instance.StartLevel();
		}

		_currentLevel = level;

		StartCoroutine(StartLevelCoroutine());
	}

	public void OnNotifyPoppedBubble()
	{
		--BubblesRemaining;
		Debug.Log("Popped bubble! Remaning: " + BubblesRemaining);
		if (BubblesRemaining == 0u)
		{
			RoundCompleted();
		}
	}

	private void RoundCompleted()
	{
		Debug.Log("Round Completed!");
		_playing = false;
        PowerUpManager.Instance.EndLevel();

        IEnumerator RoundCompletedCoroutine()
		{
			yield return new WaitForSeconds(1.0f);

			BurbujasCreator bubblesCreator = _roundsObjects[_currentLevel];
			if (_smoke != null)
			{
				AutoKill smoke = Instantiate(_smoke, bubblesCreator.transform.position, Quaternion.identity);
				yield return new WaitForSeconds(smoke.KillTime);
			}
			// Kill all remaining bubbles (ñapa que flipas)
			Bubble[] bubbles = FindObjectsByType<Bubble>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			for (int i = 0; i < bubbles.Length; ++i)
			{
				if (bubbles[i].transform.parent == bubblesCreator.transform)
				{
					Destroy(bubbles[i].gameObject);
				}
			}
			bubblesCreator.BubblesSubObject.enabled = false;
			yield return new WaitForSeconds(2.0f);

			_musicController.SetParameterForLevel(_currentLevel + 1);

			float timeElapsed = 0.0f;
			while (timeElapsed < _timeToMoveObjectBackToInitPos)
			{
				timeElapsed = Mathf.Min(timeElapsed + Time.deltaTime, _timeToMoveObjectBackToInitPos);
				bubblesCreator.transform.position = Vector3.Lerp(_objectsPlayTransform.position, _objectsInitialTransform.position, timeElapsed / _timeToMoveObjectBackToInitPos);
				yield return null;
			}
			// Destroy the object
			Destroy(bubblesCreator.gameObject);
			// Check next round
			if (_currentLevel < _levelsInfo.Length - 1)
			{
				StartLevel(_currentLevel + 1u);
			}
			else
			{
				// Last round completed
				Victory();
			}
		}
		StartCoroutine(RoundCompletedCoroutine());
	}

	private void Victory()
	{
		IEnumerator VictoryCoroutine()
		{
			yield return new WaitForSeconds(3.0f);
			_musicController.SetReadyForFinale();
		}

		Debug.Log("VICTORY!");
		_gameHUD.SetActive(false);
		StartCoroutine(VictoryCoroutine());
	}

	public void GoBackToMainMenu()
	{
		_restartCinemachine.Play();

    }

	public List<Bubble> GetCurrentObjectBubbles()
	{
		List<Bubble> toReturn = new List<Bubble>();
        Transform objectTransform = _roundsObjects[_currentLevel].transform;

		for(int i = 0; i < objectTransform.childCount; ++i )
		{
			Transform child = objectTransform.GetChild(i);

			if(child.GetComponent<Bubble>() != null)
			{
				toReturn.Add(child.GetComponent<Bubble>());
			}
		}
		return toReturn;
    }

}
