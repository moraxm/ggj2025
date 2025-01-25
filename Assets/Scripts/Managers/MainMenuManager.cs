using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public GameObject DefaultMainMenuObject = null;
	public GameObject DefaultHowToPlayObject = null;
	public GameObject DefaultCreditsObject = null;
    [HideInInspector]
    public bool IsInMainMenuMainScreen = false;

	[SerializeField]
    private MenuCameraManager _menuCameraManager = null;

	private void OnEnable()
	{
        IsInMainMenuMainScreen = true;
		EventSystem.current.SetSelectedGameObject(DefaultMainMenuObject);
		InputManager.Instance.RegisterOnBackPerformed(OnBackPressed);
	}

	private void OnDisable()
	{
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            InputManager.Instance.UnregisterOnBackPerformed(OnBackPressed);
        }
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void OnPlay()
    {
        _menuCameraManager.GoToPlay();
    }

    public void OnHowToPlay()
    {
        _menuCameraManager.GoToHowToPlay();
    }

    public void OnCredits()
    {
        _menuCameraManager.GoToCredits();
    }

    public void OnExit()
    {
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
            OnExit();
        }
        else
        {
            _menuCameraManager.GoToMainMenu();
		}
    }
}
