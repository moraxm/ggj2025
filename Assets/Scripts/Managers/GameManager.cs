using System.Collections;
using TMPro;
using UnityEngine;

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
	private Transform _objectsInitialTransform = null;
	[SerializeField]
	private Transform _objectsPlayTransform = null;
	[SerializeField]
	private TextMeshProUGUI _timeRemainingText = null;
	[SerializeField]
	private TextMeshProUGUI _bubblesRemainingText = null;
	[SerializeField]
	private float _timeToMoveObjectToPlayPos = 1.0f;
	[SerializeField]
	private float _timeToMoveObjectBackToInitPos = 1.0f;
	[SerializeField]
	private LevelInfo[] _levelsInfo = null;

	private BurbujasCreator[] _roundsObjects = null;
	private uint _currentLevel = 0u;
	private float _roundTimeRemaining = 0.0f;
	private bool _playing = false;
	private uint _bubblesRemaining = 0u;

	public bool IsPlaying => _playing;

	private float RoundTimeRemaning
	{
		get
		{
			return _roundTimeRemaining;
		}
		set
		{
			_roundTimeRemaining = value;
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
		_musicController.SetParameterForLevel(_currentLevel);
		StartLevel(0u);
	}

	private void Update()
	{
		if (_playing)
		{
			RoundTimeRemaning = Mathf.Max(RoundTimeRemaning - Time.deltaTime, 0.0f);
			if (RoundTimeRemaning <= 0.0f)
			{
				GameOver();
			}
		}
	}

	private void StartLevel(uint level)
	{
		IEnumerator StartLevelCoroutine()
		{
			yield return new WaitForSeconds(1.0f);
			Transform objectTransform = _roundsObjects[_currentLevel].transform;
			float timeElapsed = 0.0f;
			while (timeElapsed < _timeToMoveObjectToPlayPos)
			{
				timeElapsed = Mathf.Min(timeElapsed + Time.deltaTime, _timeToMoveObjectToPlayPos);
				objectTransform.position = Vector3.Lerp(_objectsInitialTransform.position, _objectsPlayTransform.position, timeElapsed / _timeToMoveObjectToPlayPos); 
				yield return null;
			}
			yield return new WaitForSeconds(2.0f);
			Debug.Log("Round started");
			_playing = true;
		}

		_currentLevel = level;
		Debug.Log("Creating round " + level);
		BubblesRemaining = _roundsObjects[level].TotalBubbles;
		RoundTimeRemaning = _levelsInfo[level].RoundTime;

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
		IEnumerator RoundCompletedCoroutine()
		{
			yield return new WaitForSeconds(1.0f);

			_musicController.SetParameterForLevel(_currentLevel + 1);

			Transform objectTransform = _roundsObjects[_currentLevel].transform;
			float timeElapsed = 0.0f;
			while (timeElapsed < _timeToMoveObjectBackToInitPos)
			{
				timeElapsed = Mathf.Min(timeElapsed + Time.deltaTime, _timeToMoveObjectBackToInitPos);
				objectTransform.position = Vector3.Lerp(_objectsPlayTransform.position, _objectsInitialTransform.position, timeElapsed / _timeToMoveObjectBackToInitPos);
				yield return null;
			}
			// Destroy the object
			Destroy(objectTransform.gameObject);
			yield return new WaitForSeconds(2.0f);
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
			// TODO: Finale here
			GoBackToMainMenu();
		}

		Debug.Log("VICTORY!");
		StartCoroutine(VictoryCoroutine());
	}

	private void GameOver()
	{
		IEnumerator GameOverCoroutine()
		{
			yield return new WaitForSeconds(3.0f);
			GoBackToMainMenu();
		}

		Debug.Log("GAME OVER!");
		_playing = false;
		StartCoroutine(GameOverCoroutine());
	}

	private void GoBackToMainMenu()
	{
		MenuCameraManager menuCameraManager = FindFirstObjectByType<MenuCameraManager>();
		if (menuCameraManager != null)
		{
			menuCameraManager.GoToMainMenu();
		}
	}
}
