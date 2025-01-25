using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _defaultObject = null;

    [SerializeField]
    private MenuCameraManager _menuCameraManager = null;

	private void Start()
	{
        InputManager.Instance.RegisterOnBackPerformed(OnExit);
        EventSystem.current.SetSelectedGameObject(_defaultObject);
	}

	private void OnDestroy()
	{
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            InputManager.Instance.UnregisterOnBackPerformed(OnExit);
        }
	}

	public void OnPlay()
    {
        _menuCameraManager.GoToPlay();
    }

    public void OnHowToPlay()
    {
        _menuCameraManager.GoToTutorial();
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
}
