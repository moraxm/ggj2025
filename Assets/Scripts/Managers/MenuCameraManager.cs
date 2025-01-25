using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MenuCameraManager : MonoBehaviour
{
    [SerializeField]
    private MainMenuManager _mainMenuManager = null;

	public CinemachineCamera MenuCamera;
    public CinemachineCamera GameplayCamera;
    public CinemachineSplineDolly Dolly;
    public float Speed = 0.5f;
    public AnimationCurve SmoothCurve;
    const int MaxPriority = 100;

    public void GoToPlay()
    {
        _mainMenuManager.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
		MenuCamera.Priority.Value = 0;
        GetComponent<PlayableDirector>().Play();
    }

    public void GoToMainMenu()
    {
        MenuCamera.Priority.Value = MaxPriority;
        GameplayCamera.Priority.Value = 0;
        StartCoroutine(GoToMenuTargetSplineValue(0.5f));
    }

    public void GoToCredits()
    {
        _mainMenuManager.enabled = false;
		StartCoroutine(GoToTargetSplineValue(1));
    }

    public void GoToTutorial()
    {
        _mainMenuManager.enabled = false;
		StartCoroutine(GoToTargetSplineValue(0));
    }

    IEnumerator GoToMenuTargetSplineValue(float TargetValue)
    {
		yield return GoToTargetSplineValue(TargetValue);
		_mainMenuManager.enabled = true;
	}

    IEnumerator GoToTargetSplineValue(float TargetValue)
    {
        float CurrentValue = Dolly.CameraPosition;
        float f = 0;

        while (Dolly.CameraPosition != TargetValue)
        {
            Dolly.CameraPosition = Mathf.Lerp(CurrentValue, TargetValue, SmoothCurve.Evaluate(f));
            f += Time.deltaTime * Speed;
            yield return null;
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GoToCredits();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            GoToTutorial();
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
