using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class MenuCameraManager : MonoBehaviour
{
    [SerializeField]
    private MainMenuManager _mainMenuManager = null;

	public CinemachineCamera MenuCamera;
    public CinemachineCamera GameplayCamera;
    public CinemachineSplineDolly Dolly;
    public float Speed = 0.5f;
    public AnimationCurve SmoothCurve;
    [HideInInspector]
    public bool IsTransitioning = false;
    const int MaxPriority = 100;

    public void GoToPlay()
    {
        _mainMenuManager.enabled = false;
		MenuCamera.Priority.Value = 0;
        GetComponent<PlayableDirector>().Play();
    }

    public void GoToMainMenu()
    {
        if (!IsTransitioning)
        {
			MenuCamera.Priority.Value = MaxPriority;
			GameplayCamera.Priority.Value = 0;
			StartCoroutine(GoToMenuTargetSplineValue(0.5f));
        }
    }

	public void GoToHowToPlay()
    {
        if (!IsTransitioning)
        {
			EventSystem.current.SetSelectedGameObject(null);
			StartCoroutine(GoToHowToPlayTargetSplineValue(0));
        }
	}

	public void GoToCredits()
    {
		if (!IsTransitioning)
		{
			EventSystem.current.SetSelectedGameObject(null);
			StartCoroutine(GoToCreditsTargetSplineValue(1));
		}
    }

    IEnumerator GoToMenuTargetSplineValue(float TargetValue)
    {
		yield return GoToTargetSplineValue(TargetValue);
        // Only when coming back from game
        if (!_mainMenuManager.enabled)
        {
            _mainMenuManager.enabled = true;
        }
        else
        {
            // Just select default object
			EventSystem.current.SetSelectedGameObject(_mainMenuManager.DefaultMainMenuObject);
            _mainMenuManager.IsInMainMenuMainScreen = true;
		}
	}

    IEnumerator GoToHowToPlayTargetSplineValue(float TargetValue)
    {
        yield return GoToTargetSplineValue(TargetValue);
        EventSystem.current.SetSelectedGameObject(_mainMenuManager.DefaultHowToPlayObject);
        _mainMenuManager.IsInMainMenuMainScreen = false;
	}

	IEnumerator GoToCreditsTargetSplineValue(float TargetValue)
	{
		yield return GoToTargetSplineValue(TargetValue);
		EventSystem.current.SetSelectedGameObject(_mainMenuManager.DefaultCreditsObject);
		_mainMenuManager.IsInMainMenuMainScreen = false;
	}

    IEnumerator GoToTargetSplineValue(float TargetValue)
    {
        IsTransitioning = true;
        float CurrentValue = Dolly.CameraPosition;
        float f = 0;

        while (Dolly.CameraPosition != TargetValue)
        {
            Dolly.CameraPosition = Mathf.Lerp(CurrentValue, TargetValue, SmoothCurve.Evaluate(f));
            f += Time.deltaTime * Speed;
            yield return null;
        }
		IsTransitioning = false;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GoToCredits();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            GoToHowToPlay();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            GoToMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            GoToPlay();
        }
    }
}
