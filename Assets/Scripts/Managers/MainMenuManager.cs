using FMODUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public GameObject DefaultMainMenuObject = null;
	public GameObject DefaultHowToPlayObject = null;
	public GameObject DefaultCreditsObject = null;
    [SerializeField]
    private EventReference _clickEvt;
	[SerializeField]
	private EventReference _backEvt;
	[HideInInspector]
    public bool IsInMainMenuMainScreen = false;

	[SerializeField]
    private MenuCameraManager _menuCameraManager = null;
	[SerializeField]
	private Canvas _canvas = null;

	private void OnEnable()
	{
        IsInMainMenuMainScreen = true;
        _canvas.enabled = true;
		EventSystem.current.SetSelectedGameObject(DefaultMainMenuObject);
		InputManager.Instance.RegisterOnBackPerformed(OnBackPressed);
	}

	private void OnDisable()
	{
        _canvas.enabled = false;
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            InputManager.Instance.UnregisterOnBackPerformed(OnBackPressed);
        }
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
	}

	public void OnPlay()
    {
        if (!_menuCameraManager.IsTransitioning && IsInMainMenuMainScreen)
        {
            AudioManager.Instance.PlayOneShot(_clickEvt);
            _menuCameraManager.GoToPlay();
        }
    }

    public void OnHowToPlay()
    {
        if (!_menuCameraManager.IsTransitioning && IsInMainMenuMainScreen)
        {
            AudioManager.Instance.PlayOneShot(_clickEvt);
            _menuCameraManager.GoToHowToPlay();
        }
    }

    public void OnCredits()
    {
        if (!_menuCameraManager.IsTransitioning && IsInMainMenuMainScreen)
        {
            AudioManager.Instance.PlayOneShot(_clickEvt);
            _menuCameraManager.GoToCredits();
        }
    }

    public void OnExit()
    {
        if (!_menuCameraManager.IsTransitioning && IsInMainMenuMainScreen)
        {
            AudioManager.Instance.PlayOneShot(_clickEvt);
            Exit();
        }
	}

    private void Exit()
    {
		AudioManager.Instance.PlayOneShot(_backEvt);
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#elif !UNITY_ANDROID
        Application.Quit();
#endif
	}

	public void OnBackPressed()
    {
        if (_menuCameraManager.IsTransitioning)
        {
            return;
        }

        if (IsInMainMenuMainScreen)
        {
			Exit();
        }
        else
        {
            AudioManager.Instance.PlayOneShot(_backEvt);
            _menuCameraManager.GoToMainMenu();
		}
    }
}
